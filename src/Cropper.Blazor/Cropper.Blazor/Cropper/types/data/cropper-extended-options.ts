import Cropper, { CropperOptions } from "cropperjs";

export namespace CropperBlazor.Data {
  type CropperAction =
    "none"
    | "select"
    | "move"
    | "scale"
    | "rotate"
    | "transform"
    | "n-resize"
    | "e-resize"
    | "s-resize"
    | "w-resize"
    | "ne-resize"
    | "nw-resize"
    | "se-resize"
    | "sw-resize";

  type CanvasElementOptions = {
    background?: boolean | null;
    disabled?: boolean | null;
    hidden?: boolean | null;
    scaleStep?: number | null;
    slottable?: boolean | null;
    themeColor?: string | null;
  };

  type ImageElementOptions = {
    alt?: string | null;
    hidden?: boolean | null;
    initialCenterSize?: "contain" | "cover" | null;
    rotatable?: boolean | null;
    scalable?: boolean | null;
    skewable?: boolean | null;
    slottable?: boolean | null;
    translatable?: boolean | null;
  };

  type ShadeElementOptions = {
    height?: number | null;
    hidden?: boolean | null;
    slottable?: boolean | null;
    themeColor?: string | null;
    width?: number | null;
    x?: number | null;
    y?: number | null;
  };

  type HandleElementOptions = {
    action?: CropperAction | null;
    hidden?: boolean | null;
    plain?: boolean | null;
    slottable?: boolean | null;
    themeColor?: string | null;
  };

  type SelectionElementOptions = {
    aspectRatio?: number | null;
    dynamic?: boolean | null;
    height?: number | null;
    hidden?: boolean | null;
    initialAspectRatio?: number | null;
    initialCoverage?: number | null;
    keyboard?: boolean | null;
    movable?: boolean | null;
    multiple?: boolean | null;
    outlined?: boolean | null;
    precise?: boolean | null;
    resizable?: boolean | null;
    slottable?: boolean | null;
    width?: number | null;
    x?: number | null;
    y?: number | null;
    zoomable?: boolean | null;
  };

  type GridElementOptions = {
    bordered?: boolean | null;
    columns?: number | null;
    covered?: boolean | null;
    hidden?: boolean | null;
    rows?: number | null;
    slottable?: boolean | null;
    themeColor?: string | null;
  };

  type CrosshairElementOptions = {
    centered?: boolean | null;
    hidden?: boolean | null;
    slottable?: boolean | null;
    themeColor?: string | null;
  };

  type ResizeHandleElementOptions = {
    hidden?: boolean | null;
    slottable?: boolean | null;
    themeColor?: string | null;
  };

  export type CropperExtendedOptions =
    CropperOptions & {
      correlationId?: string;
      aspectRatio?: number | null;
      autoCrop?: boolean | null;
      autoCropArea?: number | null;
      background?: boolean | null;
      canvasOptions?: CanvasElementOptions | null;
      canvasHidden?: boolean | null;
      canvasThemeColor?: string | null;
      center?: boolean | null;
      cropBoxMovable?: boolean | null;
      cropBoxResizable?: boolean | null;
      crosshairCentered?: boolean | null;
      crosshairOptions?: CrosshairElementOptions | null;
      crosshairHidden?: boolean | null;
      crosshairThemeColor?: string | null;
      dragMode?: "crop" | "move" | "none" | null;
      disabled?: boolean | null;
      gridBordered?: boolean | null;
      gridColumns?: number | null;
      gridCovered?: boolean | null;
      gridHidden?: boolean | null;
      gridOptions?: GridElementOptions | null;
      gridRows?: number | null;
      gridThemeColor?: string | null;
      handleAction?: CropperAction | null;
      handleHidden?: boolean | null;
      handleOptions?: HandleElementOptions | null;
      handlePlain?: boolean | null;
      handleThemeColor?: string | null;
      guides?: boolean | null;
      highlight?: boolean | null;
      imageAlt?: string | null;
      imageHidden?: boolean | null;
      imageInitialCenterSize?: "contain" | "cover" | null;
      imageOptions?: ImageElementOptions | null;
      initialAspectRatio?: number | null;
      minCanvasHeight?: number | null;
      minCanvasWidth?: number | null;
      minCropBoxHeight?: number | null;
      minCropBoxWidth?: number | null;
      modal?: boolean | null;
      movable?: boolean | null;
      moveHandleAction?: CropperAction | null;
      moveHandleHidden?: boolean | null;
      moveHandleOptions?: HandleElementOptions | null;
      moveHandleThemeColor?: string | null;
      rotatable?: boolean | null;
      scalable?: boolean | null;
      selectionDynamic?: boolean | null;
      selectionHidden?: boolean | null;
      selectionMaximumCount?: number | null;
      selectionMultiple?: boolean | null;
      selectionOptions?: SelectionElementOptions | null;
      selectionPrecise?: boolean | null;
      shadeHidden?: boolean | null;
      shadeOptions?: ShadeElementOptions | null;
      shadeThemeColor?: string | null;
      skewable?: boolean | null;
      keyboard?: boolean | null;
      outlined?: boolean | null;
      preview?: string | Element | Element[] | NodeListOf<Element> | null;
      resizeHandleHidden?: boolean | null;
      resizeHandleOptions?: ResizeHandleElementOptions | null;
      resizeHandleThemeColor?: string | null;
      zoomOnTouch?: boolean | null;
      zoomOnWheel?: boolean | null;
      zoomable?: boolean | null;
      setDataOptions?: {
        x?: number | null;
        y?: number | null;
        width?: number | null;
        height?: number | null;
        rotate?: number | null;
        scaleX?: number | null;
        scaleY?: number | null;
      } | null;
      wheelZoomRatio?: number | null;
    };
}
