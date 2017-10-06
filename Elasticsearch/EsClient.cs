using Elasticsearch.ViewModels;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Elasticsearch
{
    public class EsClient
    {
        private const string DefaultIndexName = "kk";
        private const string ElasticSearchServerUri = @"http://27.254.94.186:9200";
        private static ConnectionSettings CreateConnectionSettings()
        {
            var uri = new Uri(ElasticSearchServerUri);
            var settings = new ConnectionSettings(uri)
                .DefaultIndex(DefaultIndexName);

            return settings;
        }
        public IElasticClient CreateElasticClient()
        {
            var settings = CreateConnectionSettings();
            var client = new ElasticClient(settings);

            //check not found index
            if (client.IndexExists(DefaultIndexName).Exists)
            {
                client.DeleteIndex(DefaultIndexName);
            }

            client.CreateIndex(DefaultIndexName, c => c
                    .Settings(s => s
                        .Analysis(a => a
                            .Analyzers(aa => aa
                                .Custom("my_ngram", cs => cs
                                     .Tokenizer("my_ngram"))
                            ).Tokenizers(tk => tk
                                .NGram("my_ngram", ng => ng
                                     .MinGram(1)))
                        )
                    )
                    .Mappings(m => m
                        .Map<Content>(mm => mm
                            .Properties(p => p
                                .Text(t => t
                                    .Name(n => n.content_desc)
                                    .Analyzer("my_ngram"))))));

            return client;
        }
        public void InsertContent()
        {
            var client = CreateElasticClient();

            var content = new Content
            {
                content_id = 1,
                contenttype_id = 1,
                content_url = "http://demokk.clicknext.co.th/th/prosonalbanking/null/Debit-Card/KK-ATM-Card",
                content_name = "KK Debit Card แบบมีชิป",
                content_desc= "เพราะการทำธุรกรรมการเงินผ่านบัตรเดบิต คือ ส่วนหนึ่งของชีวิตคนไทยในยุคดิจิทัล ธนาคารเกียรตินาคินจึงได้พัฒนาบัตร KK Debit Card เพื่อตอบรับไลฟ์สไตล์การกิน ดื่ม ช้อปปิ้ง หรือกดเงินสดได้ทุกที่ทั่วโลก ผ่านเทคโนโลยี EMV Chip ที่ช่วยป้องกันการโจรกรรมข้อมูลบัตรเดบิตได้อย่างปลอดภัยตามมาตรฐานสากล",
                content_kwd= "KK Debit Card ",
                content_json= "",
                content_display=new ContentDisplay
                {
                    content_id = 1,
                    contenttype_id = 1,
                    content_url = "http://demokk.clicknext.co.th/th/prosonalbanking/null/Debit-Card/KK-ATM-Card",
                    content_name = "KK Debit Card แบบมีชิป",
                    content_desc = "เพราะการทำธุรกรรมการเงินผ่านบัตรเดบิต คือ ส่วนหนึ่งของชีวิตคนไทยในยุคดิจิทัล ธนาคารเกียรตินาคินจึงได้พัฒนาบัตร KK Debit Card เพื่อตอบรับไลฟ์สไตล์การกิน ดื่ม ช้อปปิ้ง หรือกดเงินสดได้ทุกที่ทั่วโลก ผ่านเทคโนโลยี EMV Chip ที่ช่วยป้องกันการโจรกรรมข้อมูลบัตรเดบิตได้อย่างปลอดภัยตามมาตรฐานสากล",
                    content_kwd = "KK Debit Card ",
                    content_json = "",
                }
            };

            client.Index(content, idx => idx.Index("kk"));
        }
        public void UpdateContent(string _id)
        {
            //Delete Old Data
            DeleteContent(_id);

            //Insert New Data
            InsertContent();
        }
        public void DeleteContent(string _id)
        {
            var client = CreateElasticClient();

            client.Delete(new DeleteRequest("kk", "content", _id));
        }
        public ResultES GetContent(string _id)
        {
            var client = CreateElasticClient();

            var request = client.Get<Content>(_id).Source;

            ResultES result = new ResultES();
            if (request == null)
            {
                result.ResponseCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                result.ResponseCode = (int)HttpStatusCode.OK;
                result.Data = request;
            }
            
            return result;
        }
        public ResultES SearchAll(string search)
        {
            var client = CreateElasticClient();

            //Content
            //var content = client.Search<Content>(s => s
            //      .AllTypes()
            //      .Query(q => q
            //          .Bool(b => b
            //              .Must(m => m
            //                  .QueryString(qs => qs
            //                      .DefaultField("_all")
            //                      .Query(search))))));
            //var content = client.Search<Content>(s => s
            //      .AllTypes()
            //      .Query(q => q
            //          .QueryString(qs => qs.Query(search)
            //      )));
            var content = client.Search<Content>(s => s
                  .Query(q => q
                      .QueryString(qs => qs
                          .Query(search)
                          .Fields(fs => fs
                              .Fields(f1 => f1.content_name)))));

            ResultES result = new ResultES();
            if (content.Hits.Count() == 0)
            {
                result.ResponseCode = (int)HttpStatusCode.NotFound;
            }else
            {
                result.ResponseCode = (int)HttpStatusCode.OK;
                result.Data = content.Hits.ToList();
            }

            return result;
        }
    }
}