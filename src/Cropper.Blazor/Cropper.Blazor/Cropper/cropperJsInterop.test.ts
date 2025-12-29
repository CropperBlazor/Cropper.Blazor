import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { CropperDecorator } from './cropperJsInterop';
import { CropperBlazor } from './helpers/cropper-url-image-helper';


describe('CropperDecorator', () => {
  it('should be instantiable', () => {
    const instance = new CropperDecorator();
    expect(instance).toBeInstanceOf(CropperDecorator);
  });
});