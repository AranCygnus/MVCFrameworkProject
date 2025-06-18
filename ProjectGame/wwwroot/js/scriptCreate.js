// Preview image when file input changes (Create)
document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.getElementById('formFile');
    const previewContainer = document.getElementById('previewContainer');
    const previewImg = document.getElementById('imgPreview');

    if (!fileInput || !previewImg || !previewContainer) return;

    fileInput.addEventListener('change', function () {
        const file = this.files?.[0];

        if (!file) {
            previewContainer.style.display = 'none';
            previewImg.src = '';
            return;
        }

        const reader = new FileReader();
        reader.onload = function (e) {
            previewContainer.style.display = 'block';
            previewImg.src = e.target.result;
        };
        reader.readAsDataURL(file);
    });
});
