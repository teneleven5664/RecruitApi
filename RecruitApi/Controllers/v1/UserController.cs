using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitApi.Models;
using RecruitApi.Models.DTO;
using RecruitApi.Repository;
using System.Net;
using System.Text.Json;

namespace RecruitApi.Controllers.v1
{
    [Route("api/[controller]/[action]/v{version:apiVersion}")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {

        protected APIResponse _response;
        private readonly IRecruitRepository _recruitRepository; 
        private readonly IMapper _mapper;
        public UserController(IRecruitRepository recruitRepository, IMapper mapper)
        {
            _recruitRepository = recruitRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetUsers([FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<User> userList = await _recruitRepository.GetAllAsync<User>(pageSize: pageSize,
                        pageNumber: pageNumber);
   
                if (!string.IsNullOrEmpty(search))
                {
                    userList = userList.Where(u => u.UserName.ToLower().Contains(search));
                }
                
                _response.Result = _mapper.Map<List<UserDTO>>(userList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<APIResponse>> GetUser(string? id)
        {
            try
            {

                Guid.TryParse(id, out var gid);

                if (gid == Guid.Empty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Invalid ID" };
                    return BadRequest(_response);
                }


                var user = await _recruitRepository.GetAsync<User>(u => u.Id == gid);
                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody] UserDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Invalid Data" };
                    return BadRequest(_response);
                }
                if (await _recruitRepository.GetAsync<User>(u => u.UserName.ToLower() == userDTO.UserName.ToLower()) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "User already Exists!" };
                    return BadRequest(_response);
                }

                if (userDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Invalid Data" };
                    return BadRequest(_response);
                }

                User user = _mapper.Map<User>(userDTO);
                user.Id = Guid.NewGuid();
                user.CreatedByUserId = new Guid("C099F864-4D3B-4F3A-8B22-616CB72CF79B");
                user.CreationDateTime = DateTime.Now;

                
                await _recruitRepository.CreateAsync<User>(user);
                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


    }
}
