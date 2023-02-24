//WSYJA
(function (window) {
    /**
    **  To use start with the 'init(conatiner_id/container_class)'
    **
    **
    **/
    // You can enable the strict mode commenting the following line  
    //'use strict';

    // Main library 
    function myLibrary() {
        let _myLibraryObject = {};
        //  Private variables
        let defaults = {
            container: null,
            html: `<div id="code-editor">
                <div class="editor-block-controls">
                  <div>
                    <button type="button" class="command" title="Bold" data-command='bold'><i class="fas fa-bold"></i></button>
                    <button type="button" class="command" title="Title" data-command='italic'><i class="fas fa-italic"></i></button>
                    <button type="button" class='command' title="Strike-Through" data-command='strikethrough'><strike>S</strike></button>
                    <button type="button" class="command" title="Blockquote" data-command='subscript'>X<sub>2</sub></button>
                    <button type="button" class="command" title="Superscript" data-command='superscript'>X<sup>2</sup></button>
                    <button type="button" class="command" title="Underline" data-command='underline'>U</button>
                  </div>
                  
                </div>
                
              <div id="editor-block-content" contenteditable placeholder="write"></div>
              </div>`
        };

        // Methods of myLibrary
        _myLibraryObject.init = function (container) {
            defaults.container = document.querySelector(container);
            _myLibraryObject.setUI();
            _myLibraryObject.setListeners();
            return defaults.container;
        };

        _myLibraryObject.setUI = function () {
            if (defaults.container !== null && defaults.container)
                defaults.container.innerHTML = defaults.html;
        };

        _myLibraryObject.setListeners = function () {
            for (let el of defaults.container.querySelectorAll('button.command')) {
                el.addEventListener('mousedown', function (e) {
                    let command = e.currentTarget.getAttribute('data-command');
                    switch (command) {
                        case 'h1':
                        case 'h2':
                        case 'p':
                            document.execCommand('formatBlock', false, command);
                            break;
                        case 'formatBlock':
                            document.execCommand('formatBlock', false, "blockquote");
                            break;
                        case 'increasefontsize':
                            document.execCommand('fontSize', false, 5);
                            break;
                        case 'decreasefontsize':
                            document.execCommand('fontSize', false, 2);
                            break;
                        default:
                            document.execCommand(command, false, command);
                            break;
                    }
                });
            }
            // var a = document.getElementById("editor-block-content");
            // a.addEventListener("blur", function(){

            //     console.log(a.innerText);
            // })

            let selected = defaults.container.querySelector('select');
            if (selected) {
                selected.addEventListener('change', (e) => {
                    document.execCommand('fontSize', false, e.target.value);
                });
            }

        };

        return _myLibraryObject;
    }




    // We need that our library is globally accesible, then we save in the window
    if (typeof (window.wsyJA) === 'undefined') {
        window.wsyJA = myLibrary();
    }
})(window);

wsyJA.init('.wsyig');


