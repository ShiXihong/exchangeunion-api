$(function () {
    // create sidebar and attach to menu open
    $('.ui.sidebar')
        .sidebar('attach events', '.toc.item');

    //Hack to make MVC ValidationSummary tag helper play nicely with Semantic UI
    $('.ui.error ul').addClass('list');

    if ($('.ui.error ul li:first').css('display') === 'none') {
        $('.ui.error').hide();
    } else {
        $('.ui.error').show();
    }

    $('.ui.checkbox').checkbox();
    $('.ui.dropdown').dropdown();


    $('.ui.calendar[format="date"]').calendar({
        type: 'date'
    });

    $('.message .close').on('click', function () {
        $(this).closest('.message').transition('fade');
    });

    $.FroalaEditor.DEFAULTS.key = 'VB-16cE-11aptI2C-21rs==';

    $('textarea.editor').froalaEditor({
        charCounterCount: false,
        height: 300
    });

    $('.ui.negative.button').on('click', function (e) {
        e.preventDefault();
        $('.ui.confirm.modal')
            .modal({
                closable: false,
                onApprove: function () {
                    $('.confirm form').submit();
                }
            })
            .modal('show');
    });

    $('.ui.progress').progress();

    var imgLoader = $('.js-image-upload').find("img");
    if (!imgLoader.attr("src")) {
        imgLoader.hide();
    } else {
        imgLoader.show();
    }

    $('.js-image-upload').find('input[type="file"]').fileupload({
        dataType: 'json',
        url: '/Attachment',
        progressall: function (e, data) {
            $uploader = $(this).closest('.js-image-upload');
            var percent = parseInt(data.loaded / data.total * 100, 10);
            $uploader.find('.ui.progress').show().progress({ percent: percent });
        },
        done: function (e, data) {
            $uploader = $(this).closest('.js-image-upload');

            $uploader.find('.ui.progress').hide();
            var files = data.result.files;
            if (files && files.length > 0) {
                $uploader.find('input#imageUrlValue').val(data.result.files[0]);
                $uploader.find('img').show().attr('src', data.result.files[0]);
                $uploader.find("span.field-validation-error").hide();
            }
        }
    });

    //upload whiter paper
    uploadFile('.pdf-zh', '/Attachment?pdf=zh');
    uploadFile('.pdf-en', '/Attachment?pdf=en');

    //upload json
    uploadFile('.json-zh', '/Attachment?json=zh');
    uploadFile('.json-en', '/Attachment?json=en');

    //$("#newsUploadInput").fileinput({ 'showUpload': false, 'showPreview': false, 'previewFileType': 'any' });

    $('table.reorderable').DataTable({
        filter: false,
        paging: false,
        info: false,
        rowReorder: {
            selector: 'tr'
        },
        columnDefs: [
            { targets: 0, visible: false }
        ]
    }).on('row-reorder', function (e, details) {
        var diff = details || [];
        var url = $(this).data('url');
        diff = diff.map(function (d) {
            return {
                oldPriority: parseInt(d.oldData, 10),
                newPriority: parseInt(d.newData, 10)
            };
        });
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(diff),
            dataType: 'json',
            success: function (resp) { }
        });
    }).on('row-sort', function (e, details, edit) {
        console.log('on sort');
    });


    $('a.js-delete-contact').on('click', function (e) {
        e.preventDefault();
        var $el = $(this);
        var contactId = $el.data('contactid');
        $('[name="DeleteContactID"]').val(contactId);

        $el.closest('form').submit();
    });

    $('.select-currency .checkbox')
        .checkbox({
            onChecked: function () {
                showInput($(this), true);
            },
            onUnchecked: function () {
                showInput($(this), false);
            }
        })

    $('.select-currency .checkbox input[type=checkbox]').each(function () {
        if ($(this).attr("checked")) {
            var name = $(this).attr("name");
            $("div.link-input[data-name=" + name + "]").show();
        }
    });

    PostTranslation();

    DeployI18n();
});


function showInput(ele, avtive) {
    var name = ele.attr("name");
    if (avtive) {
        $("div.link-input[data-name=" + name + "]").show();
    } else {
        $("div.link-input[data-name=" + name + "] input[type=text]").val('');
        $("div.link-input[data-name=" + name + "]").hide();
    }

}


function uploadFile(className, ulr) {
    $(className).find('input[type="file"]').fileupload({
        dataType: 'json',
        url: ulr,
        progressall: function (e, data) {
            $uploader = $(this).closest(className);
            $(className).find("div.info").show();
            if (className.indexOf("json") != -1) {
                if (data.loaded = 1) {
                    $uploader.find("div.info").find("label").text("Being analyzed, please awiat...");
                } else {
                    $(className).find("div.info").find("label").text("Uploading,please awiat....")
                }
            } else {
                var percent = parseInt(data.loaded / data.total * 100, 10);
                $uploader.find('.ui.progress').show().progress({ percent: percent });
            }
        },
        done: function (e, data) {
            $uploader = $(this).closest(className);

            $uploader.find("div.info").find("label").text("complate");

            $uploader.find('.ui.progress').hide();
            var files = data.result.files;
            if (files && files.length > 0) {
                $uploader.find("div.info").text("upload success");

                setTimeout(function () {
                    window.location.reload();
                }, 1000)
            }
        }
    });
}

function PostTranslation() {
    $("button#update_translation").on("click", function () {
        var key = $("div.field.zh_CN").attr("data-key");

        var zh_CN_id = $("div.field.zh_CN").attr("data-id");
        var zh_CN_Value = $("div.field.zh_CN").find("textarea").val();

        var en_US_id = $("div.field.en_US").attr("data-id");
        var en_US_Value = $("div.field.en_US").find("textarea").val();

        var data = [{
            Id: zh_CN_id,
            Key: key,
            Value: zh_CN_Value,
            Language: 'zh_CN'
        }, {
            Id: en_US_id,
            Key: key,
            Value: en_US_Value,
            Language: 'en_US'
            }];

        var setMessage = function (success, message) {
            $("label#update_info").addClass(success ? "success" : "error").text(message);
            setTimeout(function () {
                $("label#update_info").text("");
            }, 3000)
        }

        $.ajax("/UpdataTranslation", {
            type: 'POST',
            data: JSON.stringify(data),
            dataType: 'json',
            contentType: 'application/json',
            success: function (res) {
                setMessage(true, "Update success")
            },
            error: function (e) {
                setMessage(false, "Update error")
                console.error(e);
            }
        })
        
    })
}

function DeployI18n() {
    $("button#translationDeploy").on("click", function () {
        $("button#translationDeploy").attr("disabled", "disabled");
        $(".translationInfo").show();
        $.ajax("/Translation/Deploy", {
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            success: function (res) {
                $(".translationInfo").html("deploy success");
                setTimeout(function () {
                    window.location.reload();
                }, 1000)
            },
            error: function (e) {
                $(".translationInfo").hide();
                console.error(e);
            }
        })
    })
}