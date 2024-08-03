let timer
window.addEventListener('resize', () => {
  if (!Object.hasOwn(this, 'cropper') || cropper == null || cropper.cropperInstances == null) {
    return
  }
  const keys = Object.keys(cropper.cropperInstances)
  clearTimeout(timer)
  if (keys.length > 0) {
    keys.forEach((key) => {
      cropper.cropperInstances[key].disable()
    })
    timer = setTimeout(() => {
      const keys = Object.keys(cropper.cropperInstances)
      keys.forEach((key) => {
        cropper.cropperInstances[key].enable()
      })
    }, 100)
  }
})
