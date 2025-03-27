# Simple JSON config handler<!-- omit from toc -->

A simple library to add a key-value pair based configuration manager to any .NET project.

## Table of contents<!-- omit from toc -->

1. [Quick start guide](#quick-start-guide)
2. [Public methods](#public-methods)
3. [Safety functions](#safety-functions)

## Quick start guide

1. Add `Config.dll` or `Config.cs` to your project
2. Set the file path where the JSON file will be saved and loaded from with `Config.FilePath`
3. Set values for new or existing keys with `Config.SetValue()`
4. Get values from existing keys with `Config.GetValue()`
5. Save your keys and values for the next session with `Config.Save()`

> Values are stored as a `string`. An appropriate parsing procedure must be used to convert it to and from other types.

> For a code example, see [ConfigExample/Program.cs](ConfigExample/Program.cs)

## Public methods

| Method | Description | Parameters | Returns |
| --- | --- | --- | --- |
| `string FilePath` | Property of the file path of the value set | `value`: File path | File path |
| `void Load()` | Load the value set from the set file path |||
| `bool KeyExists(string key)` | Check if a key exists | `key`: Unique key | True, if the key exists |
| `string GetValue(string key)` | Get the current value for a key | `key`: Unique key | Current value for the key or null |
| `string GetValue(string key, string defaultValue)` | Get the current value or a default value for a key | `key`: Unique key<br>`defaultValue`: Value to return if the key does not exist | Current value for the key or defaultValue |
| `Dictionary<string, string> GetAllValues()` | Get the entire value set || Current value set |
| `void SetValue(string key, string value)` | Set a new value for a key or create a new key with a value | `key`: Unique key<br>`value`: New value ||
| `void SetAllValues(Dictionary<string, string> values)` | Overwrite the entire value set | `values`: New value set ||
| `void Save()` | Save the current value set |||

> All the methods are static, no `Config` instance needs to be created.

## Safety functions

- Keys and values are stored in a `Dictionary`, so no duplicate keys can exist.
- The file path can include or exclude the `.json` postfix. It will be appended to it if missed.
- The file path can only be set once. Any additional attempts will throw an exception with the message `The file path has already been set.`
- `Save` must be called before a `Load` call if any values were added or changed by any of the `Set` methods. If missed, an exception will be thrown with the message `There are unsaved changes that would be lost. Call Save() first.`
- If the config file does not exist when `Load` is called, the config manager will start creating the keys and values in memory. The next call to `Save` will create the file.
- If any exception is thrown during the `Load` method, an exception will be thrown with the message `Error loading config file.` and the original exception as the `innerException`.
- The file path must be set before calling `Load`, `KeyExists`, `Save` or any of the `Get` or `Set` methods. If any of these are called without the file path being set, an exception will be thrown with the message `The file path has not been set. Set it with FilePath first.`
- If `Load` was not called before calling `KeyExists`, `Save` or any of the `Get` or `Set` methods, it will be called once automatically.
- The `GetValue` method with the `defaultValue` parameter is to provide a failsafe value retrieval.
- A call to `GetAllValues` will return a copy of the underlying `Dictionary` to protect it's values in the absence of a call to `SetAllValues`.
- If any exception is thrown during the `Save` method, an exception will be thrown with the message `Error saving config file.` and the original exception as the `innerException`.
