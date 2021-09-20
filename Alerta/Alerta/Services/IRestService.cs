using Alerta.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Alerta.Services
{
    public interface IRestService
    {
        Task<Rootobject> GetAllCites(string url);
    }
}
