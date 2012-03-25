using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sitereactor.CloudProviders.Extensions
{
    /// <summary>
    /// Extension methods for exceptions.
    /// </summary>
    /// <remarks>These Exception extension methods are primarily used for the XsltExtensions methods.</remarks>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns the Exception message as XML.
        /// </summary>
        /// <param name="ex">The Exception object.</param>
        /// <returns>An XDocument of the Exception object.</returns>
        public static XDocument ToXml(this Exception exception)
        {
            // The root element is the Exception's type
            XElement root = new XElement(exception.GetType().ToString());

            if (exception.Message != null)
            {
                root.Add(new XElement("Message", exception.Message));
            }

            if (exception.StackTrace != null)
            {
                root.Add
                (
                    new XElement("StackTrace",
                        from frame in exception.StackTrace.Split('\n')
                        let prettierFrame = frame.Substring(6).Trim()
                        select new XElement("Frame", prettierFrame))
                );
            }

            // Data is never null; it's empty if there is no data
            if (exception.Data.Count > 0)
            {
                root.Add
                (
                    new XElement("Data",
                        from entry in
                            exception.Data.Cast<DictionaryEntry>()
                        let key = entry.Key.ToString()
                        let value = (entry.Value == null) ?
                            "null" : entry.Value.ToString()
                        select new XElement(key, value))
                );
            }

            // Add the InnerException if it exists
            if (exception.InnerException != null)
            {
                root.Add
                (
                    exception.InnerException.ToXml()
                );
            }

            return root.Document;
        }

        /// <summary>
        /// Returns the Exception message as a XPathNodeIterator object.
        /// </summary>
        /// <param name="ex">The Exception object.</param>
        /// <returns>An XPathNodeIterator instance of the Exception object.</returns>
        public static XPathNodeIterator ToXPathNodeIterator(this Exception exception)
        {
            XDocument doc = exception.ToXml();
            return doc.CreateNavigator().Select("/error");
        }
    }
}
