using System.Collections.Generic;
using System.Threading.Tasks;

namespace GProof.Alerta.Services
{
    public interface IGeoLocationService
    {
        Task<Dictionary<string,double>> GetCurrentLocation();
    }
}
