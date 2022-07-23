using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.API.Controllers.Base
{
    [ApiController, Route("api/{controller}")]
    public abstract class MappedEntityController<T, TBase> : ControllerBase 
        where T : IEntity 
        where TBase : IEntity
    {
        private readonly IRepository<TBase> _repository;

        private readonly IMapper _mapper;

        protected MappedEntityController(IRepository<TBase> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected virtual TBase GetBase(T item) => _mapper.Map<TBase>(item);

        protected virtual T GetItem(TBase item) => _mapper.Map<T>(item);

        protected virtual IEnumerable<TBase> GetBase(IEnumerable<T> items) => 
            _mapper.Map<IEnumerable<TBase>>(items);

        protected virtual IEnumerable<T> GetItem(IEnumerable<TBase> items) => 
            _mapper.Map<IEnumerable<T>>(items);

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> GetItemCount() => Ok(await _repository.GetCount());

        [HttpGet("contains/id/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> ContainsId(int id) => await _repository.ContainsId(id)
            ? Ok(true)
            : NotFound(false);

        [HttpPost("contains")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> Contains(T item) =>
            await _repository.Contains(GetBase(item))
                ? Ok(true)
                : NotFound(false);

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll() => Ok(GetItem(await _repository.GetAll()));

        [HttpGet("items[[{skip:int}:{count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<T>>> Get(int skip, int count) =>
            GetItem(await _repository.Get(skip, count)) is { } items && items.Any()
                ? Ok(items)
                : NotFound();

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) =>
            await _repository.GetById(id) is { } item
                ? Ok(GetItem(item))
                : NotFound();

        protected record Page(IEnumerable<T> Items, int TotalItemCount, int PageIndex, 
            int PageSize) : IPage<T>
        {
            public int TotalPageCount => (int)Math.Ceiling((double)TotalItemCount / PageSize);
        }

        protected IPage<T> GetItems(IPage<TBase> page) => new Page(GetItem(page.Items), 
            page.TotalItemCount, page.PageIndex, page.PageSize);

        [HttpGet("page/{pageIndex:int}/{pageSize:int}")]
        [HttpGet("page[[{pageIndex:int}/{pageSize:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DataSource>> GetPage(int pageIndex, int pageSize) =>
            await _repository.GetPage(pageIndex, pageSize) is { } result && result.Items.Any()
                ? Ok(GetItems(result))
                : NotFound();

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(T item)
        {
            TBase result = await _repository.Add(GetBase(item));
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, GetItem(result));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(T item)
        {
            if (await _repository.Update(GetBase(item)) is not { } result)
            {
                return NotFound(item);
            }

            return AcceptedAtAction(nameof(GetById), new { id = result.Id }, GetItem(result));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(T item)
        {
            if (await _repository.Delete(GetBase(item)) is not { } result)
            {
                return NotFound(item);
            }

            return Ok(GetItem(result));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(int id)
        {
            if (await _repository.DeleteById(id) is not { } result)
            {
                return NotFound(id);
            }

            return Ok(GetItem(result));
        }
    }
}
