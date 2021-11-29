using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRApi.Entities;
using SignalRApi.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private StockCaller _stockCaller;
        private IHubContext<NotificationHub> _hubContext { get; set; }
        public NotificationController(IHubContext<NotificationHub> hubcontext, StockCaller stockCaller)
        {
            _hubContext = hubcontext;
            _stockCaller = stockCaller;
        }
        // GET: api/values

        // POST api/values
        [HttpPost("add")]
        public IActionResult SendNotificationToClient(Notification notification)
        {
            _hubContext.Clients.All.SendAsync("sendNotification", notification.Title, notification.Message, notification.SendDate);
            return Ok();
        }
        //[HttpGet("get")]
        //public  Task<IEnumerable<Stock>> GetAction()
        //{
        //    // _hubContext.Clients.All.SendAsync("sendNotification",);
        //    var result= _stockCaller.GetValues();
        //    return result;
        //}
        [HttpGet("getall")]
        public async Task<ConcurrentDictionary<string, Stock>> GetAction2()
        {
            // _hubContext.Clients.All.SendAsync("sendNotification",);
            var result = await _stockCaller.GetValues();
            return result;
        }
        [HttpGet("get")]
        public async Task<ConcurrentDictionary<string, Stock>> GetAction()
        {
            // _hubContext.Clients.All.SendAsync("sendNotification",);
            while (true)
            {
               var result =await GetValues();
              await  _hubContext.Clients.All.SendAsync("receiveMessage",result);
            }
        }
        public async Task<ConcurrentDictionary<string, Stock>> GetValues() 
        {
            var result = await _stockCaller.GetValues();
            await Task.FromResult(result);
            return result;
        }
    }
}

