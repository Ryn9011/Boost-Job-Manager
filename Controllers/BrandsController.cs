using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JobTracker.API.data;
using JobTracker.API.Dtos;
using JobTracker.API.Models;
using JobTracker.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobTracker.API.Data;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace JobTracker.API.Controllers
{

    [Route("api/[controller]")]
    public class BrandsController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly MessageRepository _repo;
        public BrandsController(DataContext context, IMapper mapper, MessageRepository repo)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _context.Brands.Include(j => j.Jobs).ToListAsync();

            var brandForDetailedDto = _mapper.Map<List<Brand>, List<BrandForDetailedDto>>(brands);

            return Ok(brandForDetailedDto);
        }

        [HttpGet("jobs")]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _context.Jobs.Include(p => p.Photos).ToListAsync();

            var jobForDetailedDto = _mapper.Map<List<Job>, List<JobDetailedDto>>(jobs);

            return Ok(jobForDetailedDto);
        }

        [HttpGet("jobEdit")]
        public async Task<IActionResult> GetJobsToEdit()
        {
            var jobs = await _context.Jobs.Include(p => p.Photos).ToListAsync();

            var jobForDetailedDto = _mapper.Map<List<Job>, List<JobToEditDto>>(jobs);

            return Ok(jobForDetailedDto);
        }

        [HttpGet("brand")]
        public async Task<IActionResult> GetBrandsOnly()
        {
            var brands = await _context.Brands.ToListAsync();

            var brandForDetailedDto = _mapper.Map<List<Brand>, List<BrandForDetailedDto>>(brands);

            return Ok(brandForDetailedDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] JobForCreationDto jobForCreationDto)
        {
            var brandToCreate = _mapper.Map<JobForCreationDto, Job>(jobForCreationDto);

            _context.Jobs.Add(brandToCreate);
            await _context.SaveChangesAsync();

            return Ok(brandToCreate);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob([FromBody] JobForUpdateDto jobForUpdateDto, int id)
        {
            // jobForUpdateDto.UnitPrice.Remove(0, 1);
            // decimal.Parse(jobForUpdateDto.UnitPrice);
            // jobForUpdateDto.LineValue.Remove(0, 1);
            // decimal.Parse(jobForUpdateDto.LineValue);
           
            var job = await _context.Jobs.FindAsync(id);
            _mapper.Map<JobForUpdateDto, Job>(jobForUpdateDto, job);
            job.LastModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<Job, JobForUpdateDto>(job));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            _context.Remove(job);
            await _context.SaveChangesAsync();

            return Ok(id);

        }


        [HttpPut("email")]
        public async Task<IActionResult> SendEmail([FromBody] int[] emails)
        {
            List<Message> messages = new List<Message>();
            List<string> changeList = new List<string>();

            foreach (var id in emails)
            {
                messages.Add(await this._context.Messages.FirstAsync(m => m.Id == id));
            }

            foreach (var item in messages)
            {
                changeList.Add(item.Content);
            }                        

            string email = string.Join("\n\n", changeList);

            string pattern = email.Replace('[', ' ');
            string pattern2 = pattern.Replace(']', ' ');
            string pattern3 = pattern2.Replace('\"', ' ');
            string pattern4 = pattern3.Replace('\\', ' ');
            string pattern5 = pattern4.Replace(',', '\n');
            string pattern6 = pattern5.Replace('{', ' ');
            string pattern7 = pattern6.Replace('}', ' ');
            
            var SendEmail = new SendEmail();
            SendEmail.SendEmails("Merchandise.solutions@lionco.com", "Recent changes to jobs", pattern7);

            return Ok(); 
        }

    }
}