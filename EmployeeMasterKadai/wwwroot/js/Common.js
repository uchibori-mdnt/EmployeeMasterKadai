
//モーダル画面の確認ボタン
var submitButtonHtml = '<div class="d-flex justify-content-end pb-4">' +
    '<div class="p-2">' +
    '<a asp-action="Index" class="btn btn-secondary" data-bs-dismiss="modal">キャンセル</a>' +
    '</div>' +
    '<div class="p-2">' +
    '<input type="submit" value="登録" class="btn btn-primary" />' +
    '</div>' +
    '</div>';

$('#modalFooter').html(submitButtonHtml);

// 登録ボタンorキャンセルボタンが押された時の処理
$('#modalFooter').on('click', 'input[type="submit"]', function () {
    var clickedButtonValue = $(this).val();

    if (clickedButtonValue === "登録") {
        $('form').submit();
    } else if (clickedButtonValue === "キャンセル") {
        $('#externalModal').modal('hide');
    }
});

//確認メッセージのはいいいえ
var checkButtonHtml = '<div class="d-flex justify-content-end pb-4">' +
    '<div class="p-2">' +
    '<a asp-action="Index" class="btn btn-secondary" data-bs-dismiss="modal">いいえ</a>' +
    '</div>' +
    '<div class="p-2">' +
    '<input type="submit" value="はい" class="btn btn-primary" />' +
    '</div>' +
    '</div>';

//エラーメッセージの閉じるボタン
var errorCloseButton = '<div class="d-flex justify-content-end pb-4">' +
    '<div class="p-2">' +
    '<a asp-action="Index" class="btn btn-secondary" data-bs-dismiss="modal">閉じる</a>' +
    '</div>' +
    '</div>';

function showConfirmationModal(action, message) {
    $('#messageModal').modal('show');
    $('#messageLavel').text('確認');
    $('#modalBody').text(message);
    $('#closeModalBtn').html(checkButtonHtml);

    // 登録ボタンがクリックされた時の処理を追加する
    $('#closeModalBtn').on('click', 'input[type="submit"]', function () {
        var checkedButtonValue = $(this).val();
        // ここにクリックされたボタンに応じた処理を追加
    });
}

//エラーモーダル
function showErrorModal(data, retirementFlag) {
    $('#messageModal').modal('show');
    $('#messageLavel').text('エラー');
    $('#modalBody').text('登録できませんでした。画面を確認してください。');
    $('#closeModalBtn').html(errorCloseButton);

    $('#closeModalBtn').on('click', 'input[type="submit"]', function () {
        var checkedButtonValue = $(this).val();

        if (checkedButtonValue === "閉じる") {
            $('#externalModal').modal('hide');
        }
    });

    $('#modalContentPlaceholder').html(data);
    checkFlag(retirementFlag);
    $('#retirementFlag').on('change', function () {
        checkRetirement(this);
    });
}

//確認モーダルのはい・いいえ分岐
function handleConfirmationModal(checkButtonHtml) {
    $('#closeModalBtn').html(checkButtonHtml);

    $('#closeModalBtn').on('click', 'input[type="submit"]', function () {
        var checkedButtonValue = $(this).val();

        if (checkedButtonValue === "はい") {
            $('#externalModal').modal('hide');
            location.reload();
        } else if (checkedButtonValue === "いいえ") {
            $('#externalModal').modal('hide');
        }
    });
}
