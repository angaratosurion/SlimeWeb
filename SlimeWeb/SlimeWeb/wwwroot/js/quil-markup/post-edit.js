            //Quill.register('modules/imageResize', ImageResize);

            var toolbarOptions = [
                ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
                ['blockquote', 'code-block'],

                //  [{ 'header': 1 }, { 'header': 2 }],               // custom button values
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
                [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
                [{ 'direction': 'rtl' }],                         // text direction

                [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
                [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

                [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
                [{ 'font': [] }],
                [{ 'align': [] }],
                ['image'],
                ['link'],
                ['clean']                                         // remove formatting button
            ];

            var container = document.getElementById('editor');
var options = {
    debug: 'info',
                theme: 'snow',
                 scrollingContainer: '#scrolling-container',
    modules: {
        toolbar: toolbarOptions
        ,
        imageResize: {
            displaySize: true
        }
         

    }
};
            var editor = new Quill(container, options);

            editor.on('text-change', function (delta, oldDelta, source) {
                //if (source == 'api') {
                //    console.log("An API call triggered this change.");
                //} else if (source == 'user') {
                //    console.log("A user action triggered this change.");
                //}
                //var content = document.querySelector('input[name=content');
                var content = document.querySelector('textarea[name=content');
                // content.value = editor.getContents();
                var deltaobject = editor.getContents();
                var json = JSON.stringify(deltaobject);
                content.value = json;
            });

           editor.getModule("toolbar").addHandler("image", this.uploadImageHandler);

    function uploadImageHandler()
            {
               // alert('test')

                var input = document.querySelector('#uploadImg')
                input.value = ''
                input.click()
                input.onchange = uploadImage;



            }
            async function uploadImage(event) {
                //  alert('test2')
                var form = new FormData();


                form.append('upload_file', event.target.files[0]);

                // alert();
                await $.ajax({
                    url: '@ViewBag.pathbase/Posts/UploadQuill/@(Context.Request.RouteValues["id"])',
                    type: "POST",
                  contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                   // enctype: 'multipart/form-data',
                    data: form,//event.target.files[0],
                    cache: false,
               // contentType: 'multipart/form-data'   ,
                    processData: false,
                    success: function (result) {
                       /* alert(result);*/
                      //  var imgurl = JSON.parse(result);

                        var addImageRange = editor.getSelection();
                        var newRange = 0 + (addImageRange !== null ? addImageRange.index : 0);
                        console.log(addImageRange.index);
                        console.log(newRange);

                        editor.insertEmbed(newRange, 'image', result, Quill.sources.API);

                        this.editor.setSelection(1 + newRange)
                    },
                    error: function (err) {
                        alert(err.statusText);
                    },
                     headers: {
                     //    "Content-Type": "multipart/form-data"
                    }

                });
			}
				window.addEventListener('load', function () {
                var content = document.querySelector('textarea[name=content');
            //alert(@ViewBag.CreateAction);
                    // editor.setText(content.value)
					//alert("test");
                   
            editor.setContents(JSON.parse(content.value));

             });
			





    






                //var elementothide = document.getElementById('content');

               // $.("#content").hide();