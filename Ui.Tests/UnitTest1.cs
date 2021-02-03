using System;
using System.Linq;
using Xunit;
using Bunit;
using System.Threading.Tasks;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Ui.Pages;
using Xunit.Abstractions;

namespace ui.Tests
{


    public class PipelinePageTests : TestContext
    {
        public PipelinePageTests(ITestOutputHelper output) { this.output = output; }

        private readonly ITestOutputHelper output;

        [Fact]
        public void PipelineComponent_ShowsTheDefaultPipelineName()
        {
            var cut = RenderComponent<PipelinePage>();

            var pipelineNameElem = cut.Find("#pipeline-name");
            Assert.Contains(pipelineNameElem.InnerHtml, "Name: NewPipeline1");
        }


        [Fact]
        public void PipelineComponent_LetsUserUpdateNameAndWhenSaveIsClickedShowsTheUpdatedName()
        {
            var cut = RenderComponent<PipelinePage>();

            var pipelineNameElem = cut.Find("#pipeline-name");
            Assert.Contains(pipelineNameElem.InnerHtml, "Name: NewPipeline1");

            var nameEditingDiv = cut.Find("#name-editing");
            AssertHidden(nameEditingDiv);

            var editButton = cut.Find("#pipeline-name-div button");
            editButton.Click();
            AssertVisible(nameEditingDiv);

            var newPipelineName = "Edited Name 001";
            cut.Find("#name-editing input").Change(newPipelineName);

            cut.Find("#pipeline-name-edit-apply").Click();
            AssertHidden(nameEditingDiv);
            Assert.Contains(pipelineNameElem.InnerHtml, $"Name: {newPipelineName}");

        }

        private static void AssertVisible(IElement expectedHidden)
        {
            Assert.True(expectedHidden.Attributes.Count(a => a.Name == "hidden" && a.Value == "false") == 0);
        }

        private static void AssertHidden(IElement expectedHidden)
        {
            Assert.True(expectedHidden.Attributes.Count(a => a.Name == "hidden" && a.Value == "") == 1);
        }
    }
}