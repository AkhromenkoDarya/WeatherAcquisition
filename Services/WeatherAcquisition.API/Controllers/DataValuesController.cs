using Microsoft.AspNetCore.Mvc;
using WeatherAcquisition.API.Controllers.Base;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.API.Controllers
{
    [ApiController]
    public class DataValuesController : EntityController<DataValue>
    {
        public DataValuesController(IRepository<DataValue> repository) : base(repository)
        {

        }
    }
}