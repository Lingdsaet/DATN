using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.ControllersAdmin
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaiTroController : ControllerBase
    {
        private readonly IVaiTroRepository _repo;

        public VaiTroController(IVaiTroRepository repo)
        {
            _repo = repo;
        }

        //  GET LIST 
        [HttpGet]
        public async Task<ActionResult<List<VaiTroResponseDto>>> GetAll()
        {
            var roles = await _repo.GetAllAsync();

            var result = roles.Select(x => new VaiTroResponseDto
            {
                Id = x.Id,
                Ma = x.Ma,
                Ten = x.Ten,
                MoTa = x.MoTa
            });

            return Ok(result);
        }

        //  GET BY ID (OPTIONAL) 
        [HttpGet("{id}")]
        public async Task<ActionResult<VaiTroResponseDto>> GetById(byte id)
        {
            var role = await _repo.GetByIdAsync(id);
            if (role == null)
                return NotFound("Không tìm thấy vai trò");

            return Ok(new VaiTroResponseDto
            {
                Id = role.Id,
                Ma = role.Ma,
                Ten = role.Ten,
                MoTa = role.MoTa
            });
        }
        //  CREATE
        [HttpPost]
        public async Task<IActionResult> Create(VaiTroCreateRequest request)
        {
            if (await _repo.ExistsByMaAsync(request.Ma))
                return BadRequest("Mã vai trò đã tồn tại");

            var entity = new VaiTro
            {
                Ma = request.Ma,
                Ten = request.Ten,
                MoTa = request.MoTa
            };

            await _repo.AddAsync(entity);
            return Ok("Thêm vai trò thành công");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(byte id)
        {
            await _repo.DeleteAsync(id);
            return Ok("Đã xóa vai trò");
        }
    }

}
