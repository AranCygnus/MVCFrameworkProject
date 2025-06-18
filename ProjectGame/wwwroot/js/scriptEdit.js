// Preview image when file input changes (Edit)
document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.getElementById('editFile');
    const previewImg = document.getElementById('imgEdit');

    fileInput?.addEventListener('change', function () {
        const file = this.files?.[0];
        if (!file) return;

        const reader = new FileReader();
        reader.onload = function (e) {
            previewImg.src = e.target.result;
            previewImg.alt = 'Image preview';
        };
        reader.readAsDataURL(file);
    });
});
