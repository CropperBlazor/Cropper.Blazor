import { describe, it, expect } from "vitest";
import { CropperDecorator } from "./cropperJsInterop";

describe("CropperDecorator", () => {
  it("should be instantiable", () => {
    const instance = new CropperDecorator();
    expect(instance).toBeInstanceOf(CropperDecorator);
  });
});
