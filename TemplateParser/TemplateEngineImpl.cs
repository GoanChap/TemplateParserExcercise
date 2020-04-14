using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TemplateParser {
    public class TemplateEngineImpl : ITemplateEngine {

        /// <summary>
        /// Applies the specified datasource to a string template, and returns a result string
        /// with substituted values.
        /// </summary>
        public string Apply(string template, object dataSource) {
            //TODO: Write your implementation here that passes all tests in TemplateParser.Test project        

            var outsidePropertyName = dataSource;
            var organisationPropertyName = dataSource;

            //Using RegEx to extract elements within square brackets
            string pattern = "(\\[((\\w|\\s|\\.|\"|\\/)*)\\])";
            var matches = Regex.Matches(template, pattern);
            StringBuilder sb = new StringBuilder();

            foreach (Match m in matches)
            {
                sb.Append(m);
            }

            var output = sb.ToString();

            
            foreach (string word in output.Split('[', ']', '.', ' '))
            {
                //all necessary properties will be contained within dataSource
                if (word == "" || !dataSource.ToString().Contains(word)) continue;

                if (word.Contains("Contact") && outsidePropertyName == dataSource)
                {
                    outsidePropertyName = dataSource.GetType().GetProperty(word).GetValue(dataSource,null);
                }
                else
                if (word.Contains("Organisation") && outsidePropertyName != dataSource)
                {
                    organisationPropertyName = outsidePropertyName.GetType().GetProperty(word).GetValue(outsidePropertyName,null);
                }
                else

                {                                       
                    if (organisationPropertyName == dataSource)
                    {

                        if (word == "Contact") continue;

                        var propertyName = outsidePropertyName.GetType().GetProperty(word).Name;
                        if (word.Contains(propertyName))
                        {
                            var fName = outsidePropertyName.GetType().GetProperty(propertyName).GetValue(outsidePropertyName,null).ToString();
                            if (template.Contains('.'))
                                template = template.Replace("[Contact." + word + "]", fName);
                            else
                                if (template.Contains("[" + word + "]"))
                                template = template.Replace("[" + word + "]", fName);
                            else
                                template = template.Replace("[" + word, fName + "[");
                        }
                    }
                    else
                    {
                        var propertyName = organisationPropertyName.GetType().GetProperty(word).Name;
                        if (word.Contains(propertyName))
                        {
                            var fName = organisationPropertyName.GetType().GetProperty(propertyName).GetValue(organisationPropertyName,null).ToString();
                            if (template.Contains('.'))
                                template = template.Replace("[Contact." + word + "]", fName);
                            else
                                template = template.Replace("[" + word + "]", fName);
                        }
                    }
                }

            }
                                

            template = Regex.Replace(template, pattern, "");
            
            return template;
        }
    }
}
