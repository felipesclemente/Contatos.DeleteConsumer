using Contatos.DataContracts.Commands;
using Contatos.DataContracts.Entities;
using Contatos.DeleteConsumer.Interfaces;
using Dapper;
using Npgsql;
using Serilog;

namespace Contatos.DeleteConsumer.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ContatoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Postgres") ?? string.Empty;
        }

        public async Task ApagarContatoAsync(ApagarContato contato)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                var queryString = "DELETE FROM Contato WHERE Id = @idContato";
                var result = await connection.ExecuteAsync(queryString, new { contato.IdContato });
                Console.WriteLine($"Operação concluída. Linhas afetadas: {result}.");
            }
            catch (Exception ex)
            {
                Log.Error($"ContatoRepository: Falha ao apagar o contato ID {contato.IdContato}. Exception: {ex.GetType()}. Message: {ex.Message}.");
                Console.WriteLine($"ContatoRepository: Falha ao atualizar um contato. Exception: {ex.GetType()}. Message: {ex.Message}.");
            }
        }
    }
}
