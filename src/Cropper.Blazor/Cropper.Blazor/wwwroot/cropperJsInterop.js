class CropperDecorator {
  constructor () {
    this.cropperInstances = {}
  }

  clear (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .clear()
  }

  crop (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .crop()
  }

  destroy (cropperComponentId) {
    this.cropperInstances[cropperComponentId]
      .destroy()
    delete this.cropperInstances[cropperComponentId]
  }

  disable (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .disable()
  }

  enable (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .enable()
  }

  getCanvasData (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .getCanvasData()
  }

  getContainerData (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .getContainerData()
  }

  getCropBoxData (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .getCropBoxData()
  }

  getCroppedCanvas (cropperComponentId, options) {
    options.maxWidth ??= Infinity
    options.maxHeight ??= Infinity

    return this.cropperInstances[cropperComponentId]
      .getCroppedCanvas(options)
  }

  getCroppedCanvasDataURL (cropperComponentId, options, type, encoderOptions) {
    options.maxWidth ??= Infinity
    options.maxHeight ??= Infinity

    return this.cropperInstances[cropperComponentId]
      .getCroppedCanvas(options)
      .toDataURL(type, encoderOptions)
  }

  getData (cropperComponentId, rounded) {
    return this.cropperInstances[cropperComponentId]
      .getData(rounded)
  }

  getImageData (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .getImageData()
  }

  move (cropperComponentId, offsetX, offsetY) {
    return this.cropperInstances[cropperComponentId]
      .move(offsetX, offsetY)
  }

  moveTo (cropperComponentId, x, y) {
    return this.cropperInstances[cropperComponentId]
      .moveTo(x, y)
  }

  replace (cropperComponentId, url, onlyColorChanged) {
    return this.cropperInstances[cropperComponentId]
      .replace(url, onlyColorChanged)
  }

  reset (cropperComponentId) {
    return this.cropperInstances[cropperComponentId]
      .reset()
  }

  rotate (cropperComponentId, degree) {
    return this.cropperInstances[cropperComponentId]
      .rotate(degree)
  }

  rotateTo (cropperComponentId, degree) {
    return this.cropperInstances[cropperComponentId]
      .rotateTo(degree)
  }

  scale (cropperComponentId, scaleX, scaleY) {
    return this.cropperInstances[cropperComponentId]
      .scale(scaleX, scaleY)
  }

  scaleX (cropperComponentId, scaleX) {
    return this.cropperInstances[cropperComponentId]
      .scaleX(scaleX)
  }

  scaleY (cropperComponentId, scaleY) {
    return this.cropperInstances[cropperComponentId]
      .scaleY(scaleY)
  }

  setAspectRatio (cropperComponentId, aspectRatio) {
    return this.cropperInstances[cropperComponentId]
      .setAspectRatio(aspectRatio)
  }

  setCanvasData (cropperComponentId, data) {
    return this.cropperInstances[cropperComponentId]
      .setCanvasData(data)
  }

  setCropBoxData (cropperComponentId, data) {
    return this.cropperInstances[cropperComponentId]
      .setCropBoxData(data)
  }

  setData (cropperComponentId, data) {
    return this.cropperInstances[cropperComponentId]
      .setData(data)
  }

  setDragMode (cropperComponentId, dragMode) {
    return this.cropperInstances[cropperComponentId]
      .setDragMode(dragMode)
  }

  zoom (cropperComponentId, ratio) {
    return this.cropperInstances[cropperComponentId]
      .zoom(ratio)
  }

  zoomTo (cropperComponentId, ratio, pivotX, pivotY) {
    return this.cropperInstances[cropperComponentId]
      .zoomTo(ratio, { pivotX, pivotY })
  }

  noConflict () {
    return Cropper.noConflict() // eslint-disable-line no-undef
  }

  setDefaults (options) {
    return Cropper.setDefaults(options) // eslint-disable-line no-undef
  }

  async getImageUsingStreaming (imageStream) {
    const arrayBuffer = await imageStream.arrayBuffer()
    const blob = new Blob([arrayBuffer])
    return URL.createObjectURL(blob)
  }

  revokeObjectUrl (url) {
    URL.revokeObjectURL(url)
  }

  getJSEventData (instance, correlationId) {
    return {
      isTrusted: instance.isTrusted,
      detail: this.getJSEventDataDetail(instance),
      type: instance.type,
      eventPhase: instance.eventPhase,
      bubbles: instance.bubbles,
      cancelable: instance.cancelable,
      defaultPrevented: instance.defaultPrevented,
      composed: instance.composed,
      timeStamp: instance.timeStamp,
      returnValue: instance.returnValue,
      cancelBubble: instance.cancelBubble,
      correlationId
    }
  }

  getJSEventDataDetail (instance) {
    if (instance.type === 'zoom') {
      return {
        oldRatio: instance.detail.oldRatio,
        ratio: instance.detail.ratio,
        originalEvent: instance.detail.originalEvent
          ? DotNet.createJSObjectReference(instance.detail.originalEvent) // eslint-disable-line no-undef
          : null
      }
    } else if (instance.type === 'cropstart' || instance.type === 'cropend' || instance.type === 'cropmove') {
      return {
        action: instance.detail.action,
        originalEvent: instance.detail.originalEvent
          ? DotNet.createJSObjectReference(instance.detail.originalEvent) // eslint-disable-line no-undef
          : null
      }
    }

    return instance.detail
  }

  onReady (imageObject, event, correlationId) {
    const jSEventData = this.getJSEventData(event, correlationId)
    imageObject.invokeMethodAsync('IsReady', jSEventData)
  }

  onCropStart (imageObject, event, correlationId) {
    const jSEventData = this.getJSEventData(event, correlationId)
    imageObject.invokeMethodAsync('CropperIsStarted', jSEventData)
  }

  onCropMove (imageObject, event, correlationId) {
    const jSEventData = this.getJSEventData(event, correlationId)
    imageObject.invokeMethodAsync('CropperIsMoved', jSEventData)
  }

  onCropEnd (imageObject, event, correlationId) {
    const jSEventData = this.getJSEventData(event, correlationId)
    imageObject.invokeMethodAsync('CropperIsEnded', jSEventData)
  }

  onCrop (imageObject, event, correlationId) {
    const jSEventData = this.getJSEventData(event, correlationId)
    imageObject.invokeMethodAsync('CropperIsCroped', jSEventData)
  }

  onZoom (imageObject, event, correlationId) {
    const jSEventData = this.getJSEventData(event, correlationId)
    imageObject.invokeMethodAsync('CropperIsZoomed', jSEventData)
  }

  initCropper (cropperComponentId, image, optionsImage, imageObject) {
    if (image == null) {
      throw new Error("Parameter 'image' must be is not null!")
    }

    if (optionsImage == null) {
      throw new Error("Parameter 'optionsImage' must be is not null!")
    }

    const options = {}
    const correlationId = optionsImage.correlationId

    if (imageObject != null) {
      const self = this

      options.ready = function (event) {
        self.onReady(imageObject, event, correlationId)
      }
      options.cropstart = function (event) {
        self.onCropStart(imageObject, event, correlationId)
      }
      options.cropmove = function (event) {
        self.onCropMove(imageObject, event, correlationId)
      }
      options.cropend = function (event) {
        self.onCropEnd(imageObject, event, correlationId)
      }
      options.crop = function (event) {
        self.onCrop(imageObject, event, correlationId)
      }
      options.zoom = function (event) {
        self.onZoom(imageObject, event, correlationId)
      }
    }

    if (optionsImage != null) {
      Object.entries(optionsImage)?.forEach(([key, value]) => {
        options[key] = value
      })
    }

    const cropper = new Cropper(image, options) // eslint-disable-line no-undef

    this.cropperInstances[cropperComponentId] = cropper
  }

  sendImageInChunks (cropperComponentId, options, dotNetObject, type, encoderOptions, maximumReceiveChunkSize) {
    options.maxWidth ??= Infinity
    options.maxHeight ??= Infinity

    const cropperInstance = this.cropperInstances[cropperComponentId]

    setTimeout(() => {
      cropperInstance.getCroppedCanvas(options).toBlob(async (blob) => {
        const reader = blob.stream().getReader()

        async function read (dotNetImageReceiver) {
          try {
            const { done, value } = await reader.read()

            if (done) {
              await dotNetImageReceiver.invokeMethodAsync('CompleteImageTransfer')

              return
            }

            if (maximumReceiveChunkSize) {
              for (let i = 0; i < value.length; i += maximumReceiveChunkSize) {
                const chunk = value.slice(i, i + maximumReceiveChunkSize)
                await dotNetImageReceiver.invokeMethodAsync('ReceiveImageChunk', chunk)
              }
            } else {
              await dotNetImageReceiver.invokeMethodAsync('ReceiveImageChunk', value)
            }

            read(dotNetImageReceiver)
          } catch (imageProcessingError) {
            await dotNetObject.invokeMethodAsync('HandleImageProcessingError', imageProcessingError.toString())
          }
        }

        read(dotNetObject)
      }, type, encoderOptions)
    }, 0)
  }
}

window.cropper = new CropperDecorator()
