using System;
using System.Xml;

namespace ClassLibrary.Configuration
{
    public interface IConfigurationSection
    {
        void ProcessSection(XmlNode node);
        string Type { get; }

        event EventHandler<EventArgs> OnSave;
    }
}
