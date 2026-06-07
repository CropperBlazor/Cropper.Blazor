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

  context.beginPath()
  context.moveTo(path[0] * width / 100, path[1] * height / 100)
  context.fillStyle = 'rgba(255, 255, 255, 0)'

  for (let i = 2; i < path.length; i += 2) {
    context.lineTo(path[i] * width / 100, path[i + 1] * height / 100)
  }

  context.closePath()
  context.clip()
  context.fill()
  context.globalCompositeOperation = 'lighter'
  context.drawImage(sourceCanvas, 0, 0, width, height)

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

    contextCanvas.beginPath()
    contextCanvas.moveTo(path[0] * widthCanvas / 100, path[1] * heightCanvas / 100)
    contextCanvas.fillStyle = 'rgba(255, 255, 255, 0)'

    for (let i = 2; i < path.length; i += 2) {
      contextCanvas.lineTo(path[i] * widthCanvas / 100, path[i + 1] * heightCanvas / 100)
    }

    contextCanvas.closePath()
    contextCanvas.clip()
    contextCanvas.fill()
    contextCanvas.globalCompositeOperation = 'lighter'
    contextCanvas.drawImage(sourceCanvas, 0, 0, widthCanvas, heightCanvas)

    // Convert to blob and stream in chunks
    createdCanvas.toBlob(async (blob) => {
      await window.cropper.readBlobInChunks(blob, dotNetImageReceiverRef, maximumReceiveChunkSize)
    }, 'image/png', 1)
  }, 0)
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
