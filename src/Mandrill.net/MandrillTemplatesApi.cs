using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    class MandrillTemplatesApi : IMandrillTemplatesApi
    {
        internal MandrillTemplatesApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public async Task<MandrillTemplateInfo> AddAsync(string templateName, string code, string text, bool publish, string fromEmail = null,
            string fromName = null, string subject = null, string[] labels = null)
        {
            return await MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/add.json", new MandrillTemplateRequest
            {
                Name = templateName,
                Code = code,
                Text = text,
                Publish = publish,
                FromEmail = fromEmail,
                FromName = fromName,
                Subject = subject,
                Labels = labels
            });
        }

        public async Task<MandrillTemplateInfo> UpdateAsync(string templateName, string code, string text, bool publish, string fromEmail = null,
          string fromName = null, string subject = null, string[] labels = null)
        {
            return await MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/update.json", new MandrillTemplateRequest
            {
                Name = templateName,
                Code = code,
                Text = text,
                Publish = publish,
                FromEmail = fromEmail,
                FromName = fromName,
                Subject = subject,
                Labels = labels
            });
        }

        public async Task<MandrillTemplateInfo> DeleteAsync(string templateName)
        {
            if (templateName == null) throw new ArgumentNullException("templateName");
            return await MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/delete.json", new MandrillTemplateRequest
            {
                Name = templateName,
            });
        }

        public async Task<IList<MandrillTemplateInfo>> ListAsync(string label = null)
        {
            return await MandrillApi.PostAsync<MandrillTemplateListRequest, IList<MandrillTemplateInfo>>("templates/list.json", new MandrillTemplateListRequest
            {
                Label = label,
            });
        }

        public async Task<MandrillTemplateRenderResponse> RenderAsync(string templateName, List<MandrillTemplateContent> templateContent, List<MandrillMergeVar> mergeVars)
        {
            if (templateName == null) throw new ArgumentNullException("templateName");

            return await MandrillApi.PostAsync<MandrillTemplateRenderRequest, MandrillTemplateRenderResponse>("templates/render.json",
                new MandrillTemplateRenderRequest
                {
                    TemplateName = templateName,
                    TemplateContent = templateContent,
                    MergeVars = mergeVars,
                });
        }

        public async Task<MandrillTemplateInfo> InfoAsync(string templateName)
        {
            if (templateName == null) throw new ArgumentNullException("templateName");
            return await MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/info.json", new MandrillTemplateRequest
            {
                Name = templateName,
            });
        }

        public async Task<MandrillTemplateInfo> PublishAsync(string templateName)
        {
            if (templateName == null) throw new ArgumentNullException("templateName");
            return await MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/publish.json", new MandrillTemplateRequest
            {
                Name = templateName,
            });
        }

        public async Task<IList<MandrillMessageTimeSeries>> TimeSeriesAsync(string templateName)
        {
            if (templateName == null) throw new ArgumentNullException("templateName");
            return await MandrillApi.PostAsync<MandrillTemplateRequest, List<MandrillMessageTimeSeries>>("templates/time-series.json", new MandrillTemplateRequest
            {
                Name = templateName,
            });
        }
    }
}