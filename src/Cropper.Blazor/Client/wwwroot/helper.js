window.downloadFromUrl = (options) => {
  const anchorElement = document.createElement('a')
  anchorElement.href = options.url
  anchorElement.download = options.fileName ?? ''
  anchorElement.click()
  anchorElement.remove()
}

window.getPolygonImage = (sourceCanvas, path) => {
  const canvas = document.createElement('canvas')
  const context = canvas.getContext('2d')
  const width = sourceCanvas.width
  const height = sourceCanvas.height

  canvas.width = width
  canvas.height = height
  context.imageSmoothingEnabled = true
  context.drawImage(sourceCanvas, 0, 0, width, height)
  context.globalCompositeOperation = 'destination-in'
  fillPolygonPath(context, path, width, height)

  return canvas.toDataURL('image/png', 1)
}

window.getEllipseImage = (sourceCanvas) => {
  const createdCanvas = document.createElement('canvas')
  const contextCanvas = createdCanvas.getContext('2d')
  const widthCanvas = sourceCanvas.width
  const heightCanvas = sourceCanvas.height

  createdCanvas.width = widthCanvas
  createdCanvas.height = heightCanvas
  contextCanvas.imageSmoothingEnabled = true

  contextCanvas.drawImage(sourceCanvas, 0, 0, widthCanvas, heightCanvas)
  contextCanvas.globalCompositeOperation = 'destination-in'
  contextCanvas.beginPath()
  contextCanvas.ellipse(widthCanvas / 2, heightCanvas / 2, widthCanvas / 2, heightCanvas / 2, 0 * Math.PI, 0, 180 * Math.PI, true)
  contextCanvas.fill()

  return createdCanvas.toDataURL('image/png', 1)
}

window.getEllipseImageInBackground = (sourceCanvas, dotNetImageReceiverRef, maximumReceiveChunkSize) => {
  setTimeout(() => {
    const newCanvas = document.createElement('canvas')
    const ctxCanvas = newCanvas.getContext('2d')
    const widthCanvas = sourceCanvas.width
    const heightCanvas = sourceCanvas.height

    newCanvas.width = widthCanvas
    newCanvas.height = heightCanvas
    ctxCanvas.imageSmoothingEnabled = true

    // Draw the source image
    ctxCanvas.drawImage(sourceCanvas, 0, 0, widthCanvas, heightCanvas)

    // Clip to ellipse
    ctxCanvas.globalCompositeOperation = 'destination-in'
    ctxCanvas.beginPath()
    ctxCanvas.ellipse(
      widthCanvas / 2,
      heightCanvas / 2,
      widthCanvas / 2,
      heightCanvas / 2,
      0,
      0,
      2 * Math.PI,
      true
    )
    ctxCanvas.fill()

    // Convert to blob and stream in chunks
    newCanvas.toBlob(async (blob) => {
      await window.cropper.readBlobInChunks(blob, dotNetImageReceiverRef, maximumReceiveChunkSize)
    }, 'image/png', 1)
  }, 0)
}

window.getPolygonImageInBackground = (sourceCanvas, path, dotNetImageReceiverRef, maximumReceiveChunkSize) => {
  setTimeout(() => {
    const createdCanvas = document.createElement('canvas')
    const contextCanvas = createdCanvas.getContext('2d')
    const widthCanvas = sourceCanvas.width
    const heightCanvas = sourceCanvas.height

    createdCanvas.width = widthCanvas
    createdCanvas.height = heightCanvas
    contextCanvas.imageSmoothingEnabled = true
    contextCanvas.drawImage(sourceCanvas, 0, 0, widthCanvas, heightCanvas)
    contextCanvas.globalCompositeOperation = 'destination-in'
    fillPolygonPath(contextCanvas, path, widthCanvas, heightCanvas)

    // Convert to blob and stream in chunks
    createdCanvas.toBlob(async (blob) => {
      await window.cropper.readBlobInChunks(blob, dotNetImageReceiverRef, maximumReceiveChunkSize)
    }, 'image/png', 1)
  }, 0)
}

function fillPolygonPath (context, path, width, height) {
  context.beginPath()
  context.moveTo(path[0] * width / 100, path[1] * height / 100)

  for (let i = 2; i < path.length; i += 2) {
    context.lineTo(path[i] * width / 100, path[i + 1] * height / 100)
  }

  context.closePath()
  context.fill()
}

window.fillCanvasWithRandomColors = (canvas) => {
  if (!canvas) return

  const ctx = canvas.getContext('2d')
  const width = canvas.width
  const height = canvas.height
  const imageData = ctx.createImageData(width, height)
  const data = imageData.data

  for (let i = 0; i < data.length; i += 4) {
    data[i] = Math.random() * 255 // Red
    data[i + 1] = Math.random() * 255 // Green
    data[i + 2] = Math.random() * 255 // Blue
    data[i + 3] = 255 // Alpha
  }

  ctx.putImageData(imageData, 0, 0)
}

window.cropperViewMode = (() => {
  const stateByContainer = new WeakMap()

  function attach (container, mode) {
    if (!container) return

    detach(container)

    const state = {
      mode,
      limitingImage: false,
      limitingSelection: false
    }

    state.limitImage = (event) => limitImage(container, state, event)
    state.limitSelection = (event) => limitSelection(container, state, event)

    const image = container.querySelector('cropper-image')

    if (image) {
      image.addEventListener('transform', state.limitImage)
    }

    container.querySelectorAll('cropper-selection').forEach((selection) => {
      selection.addEventListener('change', state.limitSelection)
    })

    stateByContainer.set(container, state)
  }

  function detach (container) {
    const state = stateByContainer.get(container)

    if (!state) return

    const image = container.querySelector('cropper-image')

    if (image) {
      image.removeEventListener('transform', state.limitImage)
    }

    container.querySelectorAll('cropper-selection').forEach((selection) => {
      selection.removeEventListener('change', state.limitSelection)
    })

    stateByContainer.delete(container)
  }

  function limitSelection (container, state, event) {
    if (state.mode < 1 || state.limitingSelection) return

    const canvas = container.querySelector('cropper-canvas')
    const selection = event.target

    if (!canvas || !selection) return

    const canvasRect = canvas.getBoundingClientRect()
    const selectionRect = selection.getBoundingClientRect()
    const limitedRect = limitRectToBounds(selectionRect, canvasRect)

    if (!limitedRect) return

    state.limitingSelection = true
    selection.$change(limitedRect.x, limitedRect.y, limitedRect.width, limitedRect.height)
    state.limitingSelection = false
  }

  function limitImage (container, state, event) {
    if (state.mode < 2 || state.limitingImage) return

    const canvas = container.querySelector('cropper-canvas')
    const image = event.target

    if (!canvas || !image) return

    const canvasRect = canvas.getBoundingClientRect()
    const imageRect = image.getBoundingClientRect()
    const nextMatrix = Array.isArray(event.detail?.matrix) ? [...event.detail.matrix] : null

    if (!nextMatrix) return

    const limitedMatrix = limitImageMatrix(nextMatrix, imageRect, canvasRect, state.mode === 3)

    if (!limitedMatrix) return

    event.preventDefault()
    state.limitingImage = true
    image.$setTransform(limitedMatrix[0], limitedMatrix[1], limitedMatrix[2], limitedMatrix[3], limitedMatrix[4], limitedMatrix[5])
    state.limitingImage = false
  }

  function limitRectToBounds (rect, bounds) {
    const width = Math.min(rect.width, bounds.width)
    const height = Math.min(rect.height, bounds.height)
    const left = Math.min(Math.max(rect.left, bounds.left), bounds.right - width)
    const top = Math.min(Math.max(rect.top, bounds.top), bounds.bottom - height)

    if (Math.abs(left - rect.left) < 0.5 && Math.abs(top - rect.top) < 0.5 && Math.abs(width - rect.width) < 0.5 && Math.abs(height - rect.height) < 0.5) {
      return null
    }

    return {
      x: left - bounds.left,
      y: top - bounds.top,
      width,
      height
    }
  }

  function limitImageMatrix (matrix, imageRect, canvasRect, shouldCover) {
    const next = [...matrix]
    const scaleX = Math.hypot(next[0], next[1]) || 1
    const scaleY = Math.hypot(next[2], next[3]) || 1
    const minimumScale = shouldCover
      ? Math.max(canvasRect.width / imageRect.width, canvasRect.height / imageRect.height)
      : Math.min(canvasRect.width / imageRect.width, canvasRect.height / imageRect.height)

    if (minimumScale > 1 && scaleX < minimumScale && scaleY < minimumScale) {
      const ratio = minimumScale / Math.min(scaleX, scaleY)
      next[0] *= ratio
      next[1] *= ratio
      next[2] *= ratio
      next[3] *= ratio
    }

    const offset = clampOffset(imageRect, canvasRect)
    next[4] += offset.x
    next[5] += offset.y

    return next.some((value, index) => Math.abs(value - matrix[index]) >= 0.01) ? next : null
  }

  function clampOffset (rect, bounds) {
    let x = 0
    let y = 0

    if (rect.width <= bounds.width) {
      x = bounds.left + (bounds.width - rect.width) / 2 - rect.left
    } else if (rect.left > bounds.left) {
      x = bounds.left - rect.left
    } else if (rect.right < bounds.right) {
      x = bounds.right - rect.right
    }

    if (rect.height <= bounds.height) {
      y = bounds.top + (bounds.height - rect.height) / 2 - rect.top
    } else if (rect.top > bounds.top) {
      y = bounds.top - rect.top
    } else if (rect.bottom < bounds.bottom) {
      y = bounds.bottom - rect.bottom
    }

    return { x, y }
  }

  return { attach, detach }
})()
