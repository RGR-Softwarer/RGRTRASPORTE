import 'package:json_annotation/json_annotation.dart';

part 'user_data_model.g.dart';

@JsonSerializable()
class UserDataModel {
  final String name;
  final String fullName;
  final String birthData;
  final String cpf;
  final String email;
  final String phoneNumber;

  UserDataModel({
    required this.name,
    required this.fullName,
    required this.birthData,
    required this.cpf,
    required this.email,
    required this.phoneNumber,
  });

  factory UserDataModel.fromJson(Map<String, dynamic> json) =>
      _$UserDataModelFromJson(json);

  Map<String, dynamic> toJson() => _$UserDataModelToJson(this);
}
