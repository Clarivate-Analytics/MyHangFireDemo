using Hangfire;
using Microsoft.AspNetCore.Mvc;
using MyHangFireDemo.Services;
using System;

namespace MyHangFireDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public JobController(IJobService jobService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _jobService = jobService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet("/GetNewBookJob")]
        public ActionResult CreateGetNewBookJob()
        {
            _recurringJobManager.AddOrUpdate("GetNewJobId", () => _jobService.CreateGetNewBookJob(), Cron.Minutely);
            return Ok();
        }

        [HttpGet("/GenerateNewBookReportJob")]
        public ActionResult CreateNewBookReportJob()
        {
            _recurringJobManager.AddOrUpdate("GenNewBookReportJobId", () => _jobService.CreateNewBookReportJob(), Cron.Minutely);
            return Ok();
        }

        [HttpGet("/FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _jobService.FireAndForgetJob());
            return Ok();
        }

        [HttpGet("/DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
            _backgroundJobClient.Schedule(() => _jobService.DelayedJob(), TimeSpan.FromSeconds(60));
            return Ok();
        }

        [HttpGet("/ReccuringJob")]
        public ActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _jobService.ReccuringJob(), Cron.Minutely);
            return Ok();
        }

        [HttpGet("/ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var parentJobId = _backgroundJobClient.Enqueue(() => _jobService.FireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobService.ContinuationJob());

            return Ok();
        }
    }
}
