import 'package:app_gustavo/app/settings/strings.dart';

class StringInputValidator {
  static String? validatorMinCharacters(String value) {
    if (value.isEmpty) return Strings.errorCompletionRequired;
    if (value.length >= 3) {
      return Strings.errorMinCharacters;
    }
    return null;
  }
}
