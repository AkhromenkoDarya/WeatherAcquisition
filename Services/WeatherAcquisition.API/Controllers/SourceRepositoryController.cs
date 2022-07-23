using AutoMapper;
using WeatherAcquisition.API.Controllers.Base;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Domain.Base;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.API.Controllers
{
    public class SourceRepositoryController : MappedEntityController<DataSourceInfo, DataSource>
    {
        public SourceRepositoryController(IRepository<DataSource> repository, IMapper mapper) 
            : base(repository, mapper)
        {

        }
    }
}
