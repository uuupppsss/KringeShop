window.ShowToastr = (type, message) => {
    toastr.options.toastClass = "toastr";
    if (type === "success") {
        toastr.success(message, "Ура! Успех!", { timeOut: 5000 });
    }

    if (type ==="error") {
        toastr.error(message, "Что-то пошло не так(", { timeOut: 10000 });
    }
}