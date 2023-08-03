using FluentAssertions;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Services
{
    public class WebBrowserServiceTests
    {
        [Fact]
        public void Open_ForProperUri_OpensLinkInDefaultWebBrowser()
        {
            IWebBrowserService service = new WebBrowserService();
            Uri uri = new("https://www.google.pl/");

            //Act
            Action act = () => service.Open(uri);

            //Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Open_ForNullUriString_ThrowsArgumentNullException()
        {
            IWebBrowserService service = new WebBrowserService();

            //Act
            Action act = () => service.Open(new Uri(null!));

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Open_ForNullUri_ThrowsArgumentNullException()
        {
            IWebBrowserService service = new WebBrowserService();

            //Act
            Action act = () => service.Open(null!);

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}