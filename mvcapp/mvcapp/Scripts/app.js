  $(document).ready(function() {
      var button = $("#clicableButton");
      button.click(function () {
          var input = $("#textInput");
          var text = input.val();
          var div = $("#dangerousDiv");
          div.html(text);
      });
  }); 