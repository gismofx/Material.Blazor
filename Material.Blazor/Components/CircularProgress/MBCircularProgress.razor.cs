﻿using Material.Blazor.Internal;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System;
using System.Threading.Tasks;

namespace Material.Blazor
{
    /// <summary>
    /// This is a general purpose Material Theme circular progress bar. Can be determinant or
    /// indeterminant. If determinant the value needs to be between 0 and 1.
    /// </summary>
    public partial class MBCircularProgress : InputComponentFoundation<double>
    {
        private const string SVGSmallDeterminate = "<svg class=\"mdc-circular-progress__determinate-circle-graphic\" viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><circle class=\"mdc-circular-progress__determinate-circle\" cx=\"12\" cy=\"12\" r=\"8.75\" stroke-dasharray=\"54.978\" stroke-dashoffset=\"54.978\" /></svg>";
        private const string SVGMediumDeterminate = "<svg class=\"mdc-circular-progress__determinate-circle-graphic\" viewBox=\"0 0 32 32\" xmlns=\"http://www.w3.org/2000/svg\"><circle class=\"mdc-circular-progress__determinate-circle\" cx=\"16\" cy=\"16\" r=\"12.5\" stroke-dasharray=\"78.54\" stroke-dashoffset=\"78.54\" /></svg>";
        private const string SVGLargeDeterminate = "<svg class=\"mdc-circular-progress__determinate-circle-graphic\" viewBox=\"0 0 48 48\" xmlns=\"http://www.w3.org/2000/svg\"><circle class=\"mdc-circular-progress__determinate-circle\" cx=\"24\" cy=\"24\" r=\"18\" stroke-dasharray=\"113.097\" stroke-dashoffset=\"113.097\" /></svg>";

        private const string SVGSmallIndeterminate = "<svg class=\"mdc-circular-progress__indeterminate-circle-graphic\" viewBox=\"0 0 24 24\" xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"12\" cy=\"12\" r=\"8.75\" stroke-dasharray=\"54.978\" stroke-dashoffset=\"27.489\" /></svg>";
        private const string SVGMediumIndeterminate = "<svg class=\"mdc-circular-progress__indeterminate-circle-graphic\" viewBox=\"0 0 32 32\" xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"16\" cy=\"16\" r=\"12.5\" stroke-dasharray=\"78.54\" stroke-dashoffset=\"39.27\" /></svg>";
        private const string SVGLargeIndeterminate = "<svg class=\"mdc-circular-progress__indeterminate-circle-graphic\" viewBox=\"0 0 48 48\" xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"24\" cy=\"24\" r=\"18\" stroke-dasharray=\"113.097\" stroke-dashoffset=\"56.549\" /></svg>";
        
        /// <summary>
        /// Makes the progress spinner indeterminant if True.
        /// </summary>
        [Parameter] public MBCircularProgressType CircularProgressType { get; set; } = MBCircularProgressType.Indeterminate;


        /// <summary>
        /// Makes the progress spinner indeterminant if True.
        /// </summary>
        [Parameter] public MBCircularProgressSize CircularProgressSize { get; set; } = MBCircularProgressSize.Large;


        /// <summary>
        /// Sets aria-label.
        /// </summary>
        [Parameter] public string AriaLabel { get; set; } = "";


        private ElementReference ElementReference { get; set; }


        /// <summary>
        /// Set aria-valuenow to an immutable value because MB doesn't want us to re-render,
        /// however we need to allow ShouldRender to return true for class changes.
        /// </summary>
        private double IntialValue { get; set; }


        private MarkupString SVGDeterminate => CircularProgressSize switch
        {
            MBCircularProgressSize.Small => (MarkupString)SVGSmallDeterminate,
            MBCircularProgressSize.Medium => (MarkupString)SVGMediumDeterminate,
            MBCircularProgressSize.Large => (MarkupString)SVGLargeDeterminate,
            _ => throw new System.NotImplementedException(),
        };


        private MarkupString SVGIndeterminate => CircularProgressSize switch
        {
            MBCircularProgressSize.Small => (MarkupString)SVGSmallIndeterminate,
            MBCircularProgressSize.Medium => (MarkupString)SVGMediumIndeterminate,
            MBCircularProgressSize.Large => (MarkupString)SVGLargeIndeterminate,
            _ => throw new System.NotImplementedException(),
        };


        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            ForceShouldRenderToTrue = true;
            IntialValue = Value;

            ClassMapper
                .Add("mdc-circular-progress")
                .AddIf("mdc-circular-progress--small", () => CircularProgressSize == MBCircularProgressSize.Small)
                .AddIf("mdc-circular-progress--medium", () => CircularProgressSize == MBCircularProgressSize.Medium)
                .AddIf("mdc-circular-progress--large", () => CircularProgressSize == MBCircularProgressSize.Large)
                .AddIf("mdc-circular-progress--indeterminate", () => CircularProgressType == MBCircularProgressType.Indeterminate)
                .AddIf("mdc-circular-progress--closed", () => CircularProgressType == MBCircularProgressType.Closed);

            OnValueSet += OnValueSetCallback;
        }


        /// <summary>
        /// Callback for value the value setter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnValueSetCallback(object sender, EventArgs e) => InvokeAsync(() => JsRuntime.InvokeVoidAsync("material_blazor.circularProgress.setProgress", ElementReference, Value));


        /// <inheritdoc/>
        private protected override async Task InitializeMdcComponent() => await JsRuntime.InvokeVoidAsync("material_blazor.circularProgress.init", ElementReference, Value);
    }
}
