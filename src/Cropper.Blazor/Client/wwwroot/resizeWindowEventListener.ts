let timer: number

window.addEventListener('resize', () => {
  if (!window.hasOwnProperty('cropper') || window.cropper == null || window.cropper.cropperInstances == null) {
    return
  }
  const keys: string[] = Object.keys(window.cropper.cropperInstances)
  clearTimeout(timer)
  if (keys.length > 0) {
    keys.forEach((key: string) => {
      window.cropper.cropperInstances[key].disable()
    })
    timer = setTimeout(() => {
      const keys: string[] = Object.keys(window.cropper.cropperInstances)
      keys.forEach((key: string) => {
        window.cropper.cropperInstances[key].enable()
      })
    }, 100)
  }
})
