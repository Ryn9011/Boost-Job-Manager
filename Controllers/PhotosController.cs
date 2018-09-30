using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using JobTracker.API.data;
using JobTracker.API.Dtos;
using JobTracker.API.Helpers;
using JobTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JobTracker.API.Controllers
{
    [Authorize]
    [Route("api/jobs/{jobId}/photos")]
    public class PhotosController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(
            DataContext context,
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._context = context;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            var photo = _mapper.Map<PhotoToReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForJob(int jobId, PhotoForCreationDto photoDto)
        {
            var job = await _context.Jobs.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == jobId);

            if (job == null)
                return BadRequest("Could not find job");

            var file = photoDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoDto.Url = uploadResult.Uri.ToString();
            photoDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoDto);

            photo.Job = job;

            job.Photos.Add(photo);

            

            if (await _context.SaveChangesAsync() > 0)
            {
                var photoToReturn = _mapper.Map<PhotoToReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int jobId, int id)
        {
            // if (jobId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //     return Unauthorized();

            var photoFromRepo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            if (photoFromRepo == null)
                return NotFound();
            
            string stringId = photoFromRepo.Id.ToString();

            if (photoFromRepo.Id != null)
            {
                var deleteParams = new DeletionParams(stringId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                    _context.Remove(photoFromRepo);
            }

            if (photoFromRepo.Id == null)
            {
                 _context.Remove(photoFromRepo);
            }

            if (await _context.SaveChangesAsync() > 0)
                return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}