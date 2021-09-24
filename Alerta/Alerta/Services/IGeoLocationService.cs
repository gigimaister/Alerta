using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alerta.Services
{
    public interface IGeoLocationService
    {
        Task<List<double>> GetCurrentLocation();
    }
}
