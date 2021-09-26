using System.Threading.Tasks;
using GProof.Alerta.Models;

namespace GProof.Alerta.Services
{
    public interface IRestService
    {
        Task<Rootobject> GetAllCites(string url);
    }
}
