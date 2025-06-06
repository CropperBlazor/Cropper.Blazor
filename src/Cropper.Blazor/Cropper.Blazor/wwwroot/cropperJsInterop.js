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

  getCroppedCanvasInBackground (cropperComponentId, options, dotNetCanvasReceiverRef) {
    setTimeout(async () => {
      const croppedCanvas = this.getCroppedCanvas(cropperComponentId, options)
      const jsCroppedCanvasRef = DotNet.createJSObjectReference(croppedCanvas) // eslint-disable-line no-undef

      await dotNetCanvasReceiverRef.invokeMethodAsync('ReceiveCanvasReference', jsCroppedCanvasRef)
    }, 0)
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

  async readBlobInChunks (blob, dotNetImageReceiverRef, maximumReceiveChunkSize) {
    // Validate blob
    if (!(blob instanceof Blob)) {
      throw new TypeError('blob must be a valid Blob object.')
    }

    // Validate dotNetImageReceiverRef
    if (!dotNetImageReceiverRef || typeof dotNetImageReceiverRef.invokeMethodAsync !== 'function') {
      throw new TypeError('dotNetImageReceiverRef must be a valid .NET object reference with an invokeMethodAsync function.')
    }

    // Validate maximumReceiveChunkSize
    if (maximumReceiveChunkSize != null && maximumReceiveChunkSize <= 0) {
      throw new RangeError('maximumReceiveChunkSize must be greater than 0 bytes when specified.')
    }

    // By default, blob.stream() reads the blob using internal chunking (typically 65536 bytes per chunk).
    // To enforce a custom chunk size, especially to control serialized message size for JS interop or SignalR limits, we wrap it in a transformed ReadableStream.
    // This allows us to split the default chunks further to stay within a maximum size constraint (e.g., for Blazor's JS interop or SignalR message limits).
    let reader = null

    if (maximumReceiveChunkSize == null) {
      reader = blob.stream().getReader()
    } else {
      const blobStream = blob.stream().getReader()

      // Binary estimation of JSON size
      const getJsonSizeBinary = (chunk) => {
        const length = chunk.length

        // Max 3 digits for the number (0 to 255)
        const bytesPerElement = 3
        // Comma between elements
        const commas = length - 1
        // For '[' and ']'
        const brackets = 2

        return (length * bytesPerElement) + commas + brackets
      }

      // Create a custom stream that enforces max chunk size
      const transformedStream = new ReadableStream({
        async pull (controller) {
          const { done, value } = await blobStream.read()

          if (done) {
            controller.close()

            return
          }

          // Function to calculate JSON size for the current chunk using binary estimation
          let offset = 0
          let lastGoodChunkSize = maximumReceiveChunkSize

          while (offset < value.length) {
            // Start with the last known good chunk size, or the remaining length
            let chunkSize = Math.min(lastGoodChunkSize, value.length - offset)
            let chunk = value.slice(offset, offset + chunkSize)
            let jsonSize = getJsonSizeBinary(chunk)

            // If the JSON size is too large, reduce the chunk size gradually
            while (jsonSize > maximumReceiveChunkSize && chunkSize > 1) {
              // Reduce the chunk size in steps of 512 bytes, but not below 1 byte
              chunkSize = Math.max(chunkSize - 512, 1)
              chunk = value.slice(offset, offset + chunkSize)
              jsonSize = getJsonSizeBinary(chunk)

              // Stop reducing if the chunk size is already very small
              if (chunkSize <= 512) {
                break
              }
            }

            // Move the offset forward by the size of the chunk just sent with update the last good chunk size
            lastGoodChunkSize = chunkSize

            offset += chunkSize

            controller.enqueue(chunk)
          }
        }
      })

      reader = transformedStream.getReader()
    }

    try {
      while (true) {
        const { done, value } = await reader.read()
        if (done) break

        await dotNetImageReceiverRef.invokeMethodAsync('ReceiveImageChunk', value)
      }

      await dotNetImageReceiverRef.invokeMethodAsync('CompleteImageTransfer')
    } catch (error) {
      await dotNetImageReceiverRef.invokeMethodAsync('HandleImageProcessingError', error.toString())
    }
  }

  sendImageInChunks (cropperComponentId, options, dotNetImageReceiverRef, type, encoderOptions, maximumReceiveChunkSize) {
    options.maxWidth ??= Infinity
    options.maxHeight ??= Infinity

    const cropperInstance = this.cropperInstances[cropperComponentId]

    setTimeout(() => {
      cropperInstance.getCroppedCanvas(options).toBlob(async (blob) => {
        await this.readBlobInChunks(blob, dotNetImageReceiverRef, maximumReceiveChunkSize)
      }, type, encoderOptions)
    }, 0)
  }
}

window.cropper = new CropperDecorator()
