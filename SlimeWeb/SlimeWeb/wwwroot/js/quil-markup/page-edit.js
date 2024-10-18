// Make sure Quill is imported correctly
// import Quill from 'quill';
// Quill.register('modules/imageResize', ImageResize);

var toolbarOptions = [
  ['bold', 'italic', 'underline', 'strike'],
  ['blockquote', 'code-block'],
  [{ header: 1 }, { header: 2 }],
  [{ list: 'ordered' }, { list: 'bullet' }],
  [{ script: 'sub' }, { script: 'super' }],
  [{ indent: '-1' }, { indent: '+1' }],
  [{ direction: 'rtl' }],
  [{ size: ['small', false, 'large', 'huge'] }],
  [{ header: [1, 2, 3, 4, 5, 6, false] }],
  [{ color: [] }, { background: [] }],
  [{ font: [] }],
  [{ align: [] }],
  ['image'],
  ['link'],
  ['clean'],
];

var container = document.getElementById('editor');
var options = {
  debug: 'info',
  theme: 'snow',
  scrollingContainer: '#scrolling-container',
  modules: {
    toolbar: toolbarOptions,
    imageResize: {
      displaySize: true,
    },
  },
};

var editor = new Quill(container, options);

editor.on('text-change', function (delta, oldDelta, source) {
  var content = document.querySelector('textarea[name=content]');
  var deltaObject = editor.getContents();
  var json = JSON.stringify(deltaObject);
  content.value = json;
});

editor.getModule('toolbar').addHandler('image', uploadImageHandler);

function uploadImageHandler() {
  var input = document.querySelector('#uploadImg');
  input.value = '';
  input.click();
  input.onchange = uploadImage;
}

async function uploadImage(event) {
  var form = new FormData();
  form.append('upload_file', event.target.files[0]);
  var pageName = $('#Name').val();

  try {
    const result = await $.ajax({
      url: '@ViewBag.pathbase/Pages/UploadQuill/' + pageName,
      type: 'POST',
      contentType: false,
      processData: false,
      data: form,
      cache: false,
    });

    var addImageRange = editor.getSelection();
    var newRange = addImageRange ? addImageRange.index : 0;
    editor.insertEmbed(newRange, 'image', result, Quill.sources.API);
    editor.setSelection(newRange + 1);
  } catch (err) {
    console.error(err);
    alert(err.statusText);
  }
}

window.addEventListener('load', function () {
  var content = document.querySelector('textarea[name=content]');
  if (content && content.value) {
    editor.setContents(JSON.parse(content.value));
  }
});
