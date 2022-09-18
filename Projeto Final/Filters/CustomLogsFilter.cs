using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Projeto_Final.Logs;
using Projeto_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_Final.Filters
{
    public class CustomLogsFilter : IResultFilter, IActionFilter
    {
        private readonly List<int> _sucessStatusCodes;
        private readonly PlayerContext _context;
        private readonly Dictionary<int, Player> _contextDict;

        public CustomLogsFilter(PlayerContext context)
        {
            _context = context;
            _contextDict = new Dictionary<int, Player>();
            _sucessStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.Equals(context.ActionDescriptor.RouteValues["controller"], "player", StringComparison.InvariantCultureIgnoreCase))
            {
                if (context.ActionArguments.ContainsKey("id") && int.TryParse(context.ActionArguments["id"].ToString(), out int id))
                {
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase) || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase) || context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var player = _context.Find<Player>(id);
                        if (player != null)
                        {
                            var playerClone = player.Clone();
                            _contextDict.Add(id, playerClone);
                        }
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Request.Path.Value.StartsWith("/Player/", StringComparison.InvariantCulture))
            {
                if (_sucessStatusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    int id;
                    if (context.HttpContext.Request.Method.Equals("put", StringComparison.InvariantCultureIgnoreCase) || context.HttpContext.Request.Method.Equals("patch", StringComparison.InvariantCultureIgnoreCase))
                    {
                        id = int.Parse(context.HttpContext.Request.Path.ToString().Split("/").Last());

                        var afterUpdate = _context.Find<Player>(id);
                        if (afterUpdate != null)
                        {
                            Player beforeUpdate;
                            if (_contextDict.TryGetValue(id, out beforeUpdate))
                            {
                                CustomLogs.SaveLog(id, afterUpdate.Nick, context.HttpContext.Request.Method, beforeUpdate, afterUpdate);
                                _contextDict.Remove(id);
                            }
                        }
                    }
                    else if (context.HttpContext.Request.Method.Equals("delete", StringComparison.InvariantCultureIgnoreCase))
                    {
                        id = int.Parse(context.HttpContext.Request.Path.ToString().Split("/").Last());
                        Player beforeUpdate;
                        if (_contextDict.TryGetValue(id, out beforeUpdate))
                        {
                            CustomLogs.SaveLog(beforeUpdate.Id, beforeUpdate.Nick, context.HttpContext.Request.Method);
                            _contextDict.Remove(id);
                        }
                    }
                }
            }
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
