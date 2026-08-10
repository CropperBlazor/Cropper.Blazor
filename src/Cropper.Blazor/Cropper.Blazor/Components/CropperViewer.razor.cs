using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cropper.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Components
{
	/// <summary>
	/// 
	/// </summary>
	public partial class CropperViewer : IAsyncDisposable, IDisposable
	{
		[Inject] ICropperJsInterop CropperJsInterop { get; set; } = null!;

		private Guid? initializedCropperComponentId;
		private int initializedCropperStateVersion = -1;
		private int initializedActiveSelectionVersion = -1;
		private int? initializedSelectionIndex;

		/// <summary>
		/// User class names, separated by space.
		/// </summary>
		[Parameter]
		public string Class { get; set; } = null!;

		/// <summary> 
		/// Captures all additional attributes passed to the component that do not match declared [Parameter] properties.
		/// These attributes can be applied ("splatted") onto a rendered HTML element using the Razor `@attributes` directive.
		/// You can pass standard Blazor event handlers (like `@onclick`, `@oninput`, etc.) in this dictionary as well.
		/// The supported DOM events are defined in <see cref="Microsoft.AspNetCore.Components.Web.EventHandlers"/> via <see cref="EventHandlerAttribute"/>.
		/// The dictionary key should match the event name (e.g., `onclick`, `oninput`) or any valid HTML attribute.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object> InputAttributes { get; set; } = null!;

		/// <summary>
		/// Provides shared synchronization state between the cropper component and this viewer.
		/// </summary>
		[Parameter]
		public CropperState? CropperState { get; set; }

		/// <summary>
		/// Changes when the viewer should rebind to the currently active selection.
		/// </summary>
		[Parameter]
		public int ActiveSelectionVersion { get; set; }

		/// <summary>
		/// Selection index to preview. When unset, the viewer follows the active selection.
		/// </summary>
		[Parameter]
		public int? SelectionIndex { get; set; }

		private ElementReference? CropperViewerReference;

		/// <inheritdoc />
        protected override Task OnParametersSetAsync()
		{
			if (CropperState is not null)
			{
				CropperState.CropperInitialized -= OnCropperInitializedAsync;
				CropperState.CropperInitialized += OnCropperInitializedAsync;
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (CropperState?.CropperComponent is not null)
			{
				await OnCropperInitializedAsync(CropperState.CropperComponent);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			if (CropperState is not null)
			{
				CropperState.CropperInitialized -= OnCropperInitializedAsync;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public async ValueTask DisposeAsync()
		{
			Dispose();
			await ValueTask.CompletedTask;
		}

		private async ValueTask OnCropperInitializedAsync(CropperComponent cropperComponent)
		{
            if (CropperViewerReference.HasValue
				&& (initializedCropperComponentId != cropperComponent.CropperComponentId
                    || initializedCropperStateVersion != CropperState?.Version
                    || initializedActiveSelectionVersion != ActiveSelectionVersion
					|| initializedSelectionIndex != SelectionIndex))
			{
				await CropperJsInterop.InitializeViewerAsync(
					cropperComponent.CropperComponentId,
                  CropperViewerReference.Value,
                    selectionIndex: SelectionIndex);

				initializedCropperComponentId = cropperComponent.CropperComponentId;
               initializedCropperStateVersion = CropperState?.Version ?? initializedCropperStateVersion;
               initializedActiveSelectionVersion = ActiveSelectionVersion;
               initializedSelectionIndex = SelectionIndex;
			}
		}
	}
}
