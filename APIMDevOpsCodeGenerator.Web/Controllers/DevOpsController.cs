using APIMDevOpsCodeGenerator.Web.Models;
using AutoMapper;
using DevOpsResourceCreator;
using DevOpsResourceCreator.Model;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIMDevOpsCodeGenerator.Web.Controllers
{
    public class DevOpsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IProjectRepository projectRepository;
        private readonly IDevOpsRepository devOpsRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<DevOpsController> _logger;
        private readonly IConfiguration configuration;
        
        public DevOpsController(ILogger<DevOpsController> _logger, IConfiguration configuration, IMapper mapper, IProjectRepository projectRepository, IDevOpsRepository devOpsRepository, IAccountRepository accountRepository)
        {
            this._logger = _logger;
            this.configuration = configuration; 
            this.mapper = mapper;
            this.projectRepository = projectRepository;
            this.devOpsRepository = devOpsRepository;
            this.accountRepository = accountRepository;
        }

        public ActionResult Index()
        {
            

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Helper.Track(User);

            var accessToken = Task.Run(async () =>
            {
                return await HttpContext.GetTokenAsync("access_token");
            }).Result;
            Models.DevOpsModel createmodel = BindCreateModel(accessToken, new Models.DevOpsModel());

            return View(createmodel);
        }
        private Models.DevOpsModel BindCreateModel(string accessToken, Models.DevOpsModel model)
        {
            var dev = new DevOpsEntity
            {
                MemberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                Token = accessToken

            };
            var response = accountRepository.GetAll(dev);
            model.RequiredExtenions = GetExtensions();
            foreach (var item in response.value)
            {
                model.Organizations.Add(new SelectListItem { Text = item.accountName, Value = item.accountName });
            }
            
            var content = DevOpsResourceCreator.Helper.GetFromResources(@"Templates.Policies.Policies.json");
            var policyModel = JsonSerializer.Deserialize<List<PolicyCategoryModel>>(content);

            if (model.SelectedPolicies != null)
            {
                
                foreach (var item in policyModel)
                {
                    foreach (var policy in item.Policies)
                    {
                        if (model.SelectedPolicies.Any(a => a.Equals(policy.TemplateName, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            policy.IsChecked = true;
                        }
                        else
                        {
                            policy.IsChecked = false;
                        }
                    }
                }
            }

            model.PolicyCategories = policyModel;
            return model;
        }
        private static List<ExtensionModel> GetExtensions()
        {
            return new List<ExtensionModel>
            {
                new ExtensionModel{ExtensionName = "ARM OutPuts",  LicenceUrl = "https://marketplace.visualstudio.com/items/keesschollaart.arm-outputs/license" },
                new ExtensionModel{ExtensionName = "Team Project Health",  LicenceUrl = "https://marketplace.visualstudio.com/items/ms-devlabs.TeamProjectHealth/license" },
            };
        }
        public ActionResult Organizations()
        {

            var accessToken = Task.Run(async () =>
            {
                return await HttpContext.GetTokenAsync("access_token");
            }).Result;

            var createmodel = new Models.DevOpsModel();

            var model = new DevOpsResourceCreator.Model.DevOpsEntity
            {
                MemberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                Token = accessToken
            };
            var response = accountRepository.GetAll(model);

            foreach (var item in response.value)
            {
                createmodel.Organizations.Add(new SelectListItem { Text = item.accountName, Value = item.accountName });
            }



            return View(createmodel);
        }
        public JsonResult Projects(string id)
        {
            var accessToken = Task.Run(async () =>
            {
                return await HttpContext.GetTokenAsync("access_token");
            }).Result;

            TempData["Organization"] = id;
            TempData.Keep();
            var devOpsModel = new DevOpsResourceCreator.Model.DevOpsEntity
            {
                Token = accessToken,
                Organization = id,


            };
            var response = projectRepository.GetAll(devOpsModel);

            List<SelectListItem> list = new List<SelectListItem>();

            var projectRoot =
                  JsonSerializer.Deserialize<ProjectRoot>(response);

            foreach (var item in projectRoot.value)
            {
                list.Add(new SelectListItem { Text = item.name, Value = item.id });
            }
            return Json(list);
        }
        public JsonResult ServiceEndPoints(string organization, string project)
        {
            var accessToken = Task.Run(async () =>
            {
                return await HttpContext.GetTokenAsync("access_token");
            }).Result;
            var devOpsModel = new DevOpsResourceCreator.Model.DevOpsEntity
            {
                Token = accessToken,
                Organization = organization,
                ProjectName = project
            };
            var response = projectRepository.GetServiceEndPoints(devOpsModel);

            var serviceConnectionRoot = JsonSerializer.Deserialize<ServiceConnectionRoot>(response);

            List<SelectListItem> list = new List<SelectListItem>();
            if (serviceConnectionRoot.count == 0)
            {
                list.Add(new SelectListItem { Text = "Api-as-service", Value = "Api-as-service" });
            }
            else
            {
                foreach (var item in serviceConnectionRoot.value)
                {
                    list.Add(new SelectListItem { Text = item.name, Value = item.name });
                }
            }

            return Json(list);
        }
        public ActionResult Repositories(DevOpsResourceCreator.Model.DevOpsEntity model)
        {
            var accessToken = Task.Run(async () =>
            {
                return await HttpContext.GetTokenAsync("access_token");
            }).Result;
            var oraganization = TempData["Organization"] as string;
            var devOpsModel = new DevOpsEntity
            {
                Token = accessToken,
                Organization = model.Organization,
                ProjectId = model.ProjectId
            };
            var response = projectRepository.GetAll(devOpsModel);

            var projectRoot =
                  JsonSerializer.Deserialize<ProjectRoot>(response);

            var result = mapper.Map<List<DevOpsProjectModel>>(projectRoot.value);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.DevOpsModel model)
        {
            var accessToken = Task.Run(async () =>
            {
                return await HttpContext.GetTokenAsync("access_token");
            }).Result;

            try
            {
                var devOpsModel = new DevOpsEntity
                {
                    MemberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Token = accessToken,
                    ProjectId = model.ProjectName,
                    CreatePipeline = model.CreatePipeline,
                    Organization = model.OrganizationName,
                    Repository = configuration.GetSection("DevOps").GetSection("RepositoryName").Value + new Random().Next(10).ToString(),
                    ServiceEndPointName = model.ProjectName + model.RepoName,                    
                    PublisherName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    PublisherEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    ServiceConnectionName = model.ServiceConnection,
                    DeploytoUAT = model.DeploytoUAT,
                    DeploytoProduction = model.DeploytoProduction,
                    DeploytoDev=model.DeploytoDev
                };

                if (model.SelectedPolicies != null && model.SelectedPolicies.Length > 0)
                {
                    devOpsModel.SelectedPolicies = model.SelectedPolicies.ToList();
                }

                devOpsRepository.Create(devOpsModel);

                var message = "Repository created successfully";
                if (model.CreatePipeline)
                {
                    message = "Repository with pipeline created successfully";
                }
                TempData["Response"] = "Success";
                TempData["ResponseMessage"] = message;
                model = BindCreateModel(accessToken, model);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                TempData["Response"] = "Error";
                TempData["ResponseMessage"] = ex.Message;
                model = BindCreateModel(accessToken, model);
                return View("Index", model);
            }
        }
    }
}