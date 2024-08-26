using CrudUsuariosStoredProcedure.Models;
using System.Data;
using System.Data.SqlClient;

namespace CrudUsuariosStoredProcedure.Data
{
    public class DataAccess
    {
        SqlConnection _connection = null;
        //proprierade que executamos os comandos que queremos fazer na requisição
        SqlCommand _command = null;

        //abrir o websetting e pegar a string de conexão, isso abaixo nos ajuda
        public static IConfiguration Configuration { get; set; }

        //método que retorna a string de conexao
        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            return Configuration.GetConnectionString("DefaultConnection");
        }

        public List<Usuario> ListarUsuarios()
        {   
            //criei uma lista
            List<Usuario> usuarios = new List<Usuario>();
            
            //fazendo a _connection armazenar a função que ativa a string de conexão
            using (_connection = new SqlConnection(GetConnectionString()))
            {   
                //_command armazena um comando
                _command = _connection.CreateCommand();
                //colocando o tipo de comando no _command sendo do tipo que é uma storedprocedure
                _command.CommandType = CommandType.StoredProcedure;
                //o text é o nome da storedprocedure que quero executar
                _command.CommandText = "[DBO].[listar_usuarios]";

                //Abrindo a conexão
                _connection.Open();

                //iniciando um objeto de leitor para ler os dados que serão retornados de dentro
                //da procedure
                SqlDataReader reader = _command.ExecuteReader();

                //enquanto tiver informação sendo retorna(sendo retornada registro da procedure)
                //enquanto tiverem registros para serem lidos
                while(reader.Read())
                {   
                    //cada registro acaba sendo um objeto Usuario
                    Usuario usuario = new Usuario();

                    //transformar a coluna lida em um int
                    usuario.Id = Convert.ToInt32(reader["Id"]);
                    //transformar a coluna lida em uma string
                    usuario.Nome = reader["Nome"].ToString();
                    usuario.Email = reader["Email"].ToString();
                    usuario.Cargo = reader["Cargo"].ToString();
                    usuario.Sobrenome = reader["Sobrenome"].ToString();

                    //adicionando "usuario" na lista "usuarios" criada nesse método
                    usuarios.Add(usuario);
                }

                //fechando conexão
                _connection.Close();

            }

            return usuarios;
        }

        public bool Cadastrar(Usuario usuario)
        {
            int id = 0;

            using (_connection = new SqlConnection(GetConnectionString()))
            {   
                //command vai ser um comando criado
                _command = _connection.CreateCommand();
                //o tipo do comando vai ser uma stored procedure
                _command.CommandType = CommandType.StoredProcedure;
                //o comando tem como texto o nome da storedprocedure do banco de dados
                _command.CommandText = "[DBO].[inserir_usuario]";

                //Agora vamos passar os parâmetros que essa procedure pede para ser executada
                _command.Parameters.AddWithValue("@Nome", usuario.Nome);
                _command.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
                _command.Parameters.AddWithValue("@Email", usuario.Email);
                _command.Parameters.AddWithValue("@Cargo", usuario.Cargo);


                //Abrindo a conexão
                _connection.Open();

                //realiza a operação no banco de dados porém não tem retorno dos dados
                //essa procedure retorna o valor de linhas afetas na tabela
                //se retornar 0 , deu ruim
                id = _command.ExecuteNonQuery();

                //fechando a conexão
                _connection.Close();
            }

            //se for maior que zero retorna true se não retorna false
            return id > 0 ? true : false;
        }

        public Usuario BuscarUsuarioPorId(int id)
        {
            Usuario usuario = new Usuario();

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[listar_usuario_id]";

                _command.Parameters.AddWithValue("@Id", id);

                _connection.Open();

                //leitura do registro, SqlDataReader serve justamente para ler um registro
                SqlDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["Id"]);
                    usuario.Nome = reader["Nome"].ToString();
                    usuario.Sobrenome = reader["Sobrenome"].ToString();
                    usuario.Email = reader["Email"].ToString();
                    usuario.Cargo = reader["Cargo"].ToString();

                }

                _connection.Close();

            }

            return usuario;
        }

        public bool Editar(Usuario usuario)
        {
            var id = 0;

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[editar_usuario]";

                _command.Parameters.AddWithValue("@Id", usuario.Id);
                _command.Parameters.AddWithValue("@Nome", usuario.Nome);
                _command.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
                _command.Parameters.AddWithValue("@Email", usuario.Email);
                _command.Parameters.AddWithValue("@Cargo", usuario.Cargo);

                _connection.Open();

                //quando abriu o banco o sql já solta um valor de linhas afetadas da tabela
                id = _command.ExecuteNonQuery();

                _connection.Close();
            }

            return id > 0 ? true : false;
            

        }

        public bool Remover(int id)
        {
            var result = 0;

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[remover_usuario]";

                _command.Parameters.AddWithValue("@Id", id);

                _connection.Open();

                result = _command.ExecuteNonQuery();

                _connection.Close();

            }

            return result > 0 ? true : false;
        }

    }
}
