using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Testing.DomainModels;
using Testing.Repositories;
using data = Testing.Models;

namespace Testing.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStydentRepository stydentRepository;
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;

        public StudentsController(IStydentRepository stydentRepository, IMapper mapper,IImageRepository imageRepository)
        {
            this.stydentRepository = stydentRepository;
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await stydentRepository.GetStudents();

            return Ok(mapper.Map<List<Student>>(students)
            );
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}"),ActionName("GetStudentAsync")]

        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            var student = await stydentRepository.GetStudentAsync(studentId);

            if (student == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Student>(student));
        }

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await stydentRepository.Exists(studentId))
            {

                var updatedStudent = await stydentRepository.UpdateStudent(studentId, mapper.Map<data.Student>(request));
                if (updatedStudent != null)
                {
                    return Ok(mapper.Map<Student>(updatedStudent));
                }
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]

        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
           if(await stydentRepository.Exists(studentId))
            {
               var student=await stydentRepository.DeleteStudent(studentId);
                return Ok(mapper.Map<Student>(student));
            }
            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentsAsync([FromBody] AddStudentRequest request)
        {
          var student= await  stydentRepository.AddStudent(mapper.Map<data.Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.Id },
                mapper.Map<Student>(student));
        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]

        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId,IFormFile profileImage)
        {
            var validExtension = new List<string>
            {
                ".jpeg",
                ".jpg",
                ".png",
                ".gif"

            };

            if (profileImage != null && profileImage.Length > 0)
            {
                if (await stydentRepository.Exists(studentId))
                {
                    var extension = Path.GetExtension(profileImage.FileName);

                    if (validExtension.Contains(extension))
                    {

                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImagePath = await imageRepository.Upload(profileImage, fileName);

                        if (await stydentRepository.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading Image");

                    }
                }
                return BadRequest("This is not a valid Image format");
            }
            return NotFound();
        }



    }
}
