import 'package:mask_text_input_formatter/mask_text_input_formatter.dart';

class CustomMask {
  static get birthDateMask =>
      MaskTextInputFormatter(
        mask: '##/##/####',
        filter: {"#": RegExp(r'[0-9]')},
      );

  static get cpfMask =>
      MaskTextInputFormatter(
        mask: '###.###.###-##',
        filter: {"#": RegExp(r'[0-9]')},
      );

  static get phoneMask =>
      MaskTextInputFormatter(
        mask: '(##)#####-####',
        filter: {"#": RegExp(r'[0-9]')},
      );


}
