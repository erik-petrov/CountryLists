using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static CountryLists.Page;

namespace CountryLists
{
    public partial class MyPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private TaskCompletionSource<Country> taskCompletionSource;
        public Task<Country> PopupClosedTask { get { return taskCompletionSource.Task; } }
        public MyPopupPage()
        {
            this.BackgroundColor = Color.Black;
            Entry name = new Entry
            {
                Placeholder = "Nimi",
                Keyboard = Keyboard.Default
            };
            Entry capital = new Entry
            {
                Placeholder = "Capital",
                Keyboard = Keyboard.Default
            };
            Entry people = new Entry
            {
                Placeholder = "People",
                Keyboard = Keyboard.Numeric
            };
            Entry url = new Entry
            {
                Placeholder = "Url",
                Keyboard = Keyboard.Default
            };
            Button accept = new Button
            {
                Text = "Accept",
            };
            accept.Clicked += (s, e) =>
            {
                taskCompletionSource.SetResult(new Country { Nimi = name.Text, Capital = capital.Text, People = people.Text, Picture = url.Text});
                Navigation.RemovePopupPageAsync(this);
            };
            Button cancel = new Button
            {
                Text = "Cancel",
            };
            cancel.Clicked += (s, e) =>
            {
                Navigation.RemovePopupPageAsync(this);
            };
            Content = new StackLayout { Children = { name, capital, people, url, accept, cancel} };
        }

        protected override void OnAppearing() { base.OnAppearing(); taskCompletionSource = new TaskCompletionSource<Country>(); }

        protected override void OnDisappearing()
        {

            base.OnDisappearing();

        }

    // ### Methods for supporting animations in your popup page ###

    // Invoked before an animation appearing
    protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
    }
}