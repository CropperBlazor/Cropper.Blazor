import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { CropperDecorator } from './cropperJsInterop';


describe('CropperDecorator', () => {
  it('should be instantiable', () => {
    const instance = new CropperDecorator();
    expect(instance).toBeInstanceOf(CropperDecorator);
  });
});