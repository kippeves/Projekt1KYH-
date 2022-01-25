var Modal = document.getElementById('adminModal')
Modal.addEventListener('show.bs.modal', function (event) {
    var modalTitle = Modal.querySelector('.modal-title')
    var modalBodyInput = Modal.querySelector('.modal-body input')
    modalTitle.textContent = 'Admin-area'
    modalBodyInput.value = 'root'
})