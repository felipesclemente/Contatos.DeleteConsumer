using Contatos.DataContracts.Commands;

namespace Contatos.DeleteConsumer.Interfaces
{
    public interface IContatoRepository
    {
        Task ApagarContatoAsync(ApagarContato contato);
    }
}
