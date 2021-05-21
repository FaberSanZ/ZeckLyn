// This code has been based from the sample repository "cecil": https://github.com/jbevain/cecil
// Copyright (c) 2020 - 2021 Faber Leonardo. All Rights Reserved. https://github.com/FaberSanZ
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)


using System;
using System.Diagnostics;
using System.Threading;
using LSharp.IL.Collections.Generic;

namespace LSharp.IL
{

	public struct CustomAttributeArgument {

		readonly TypeReference type;
		readonly object value;

		public TypeReference Type {
			get { return type; }
		}

		public object Value {
			get { return value; }
		}

		public CustomAttributeArgument (TypeReference type, object value)
		{
			Mixin.CheckType (type);
			this.type = type;
			this.value = value;
		}
	}

	public struct CustomAttributeNamedArgument {

		readonly string name;
		readonly CustomAttributeArgument argument;

		public string Name {
			get { return name; }
		}

		public CustomAttributeArgument Argument {
			get { return argument; }
		}

		public CustomAttributeNamedArgument (string name, CustomAttributeArgument argument)
		{
			Mixin.CheckName (name);
			this.name = name;
			this.argument = argument;
		}
	}

	public interface ICustomAttribute {

		TypeReference AttributeType { get; }

		bool HasFields { get; }
		bool HasProperties { get; }
		bool HasConstructorArguments { get; }
		Collection<CustomAttributeNamedArgument> Fields { get; }
		Collection<CustomAttributeNamedArgument> Properties { get; }
		Collection<CustomAttributeArgument> ConstructorArguments { get; }
	}

	[DebuggerDisplay ("{AttributeType}")]
	public sealed class CustomAttribute : ICustomAttribute {

		internal CustomAttributeValueProjection projection;
		readonly internal uint signature;
		internal bool resolved;
		MethodReference constructor;
		byte [] blob;
		internal Collection<CustomAttributeArgument> arguments;
		internal Collection<CustomAttributeNamedArgument> fields;
		internal Collection<CustomAttributeNamedArgument> properties;

		public MethodReference Constructor {
			get { return constructor; }
			set { constructor = value; }
		}

		public TypeReference AttributeType {
			get { return constructor.DeclaringType; }
		}

		public bool IsResolved {
			get { return resolved; }
		}

		public bool HasConstructorArguments {
			get {
				Resolve ();

				return !arguments.IsNullOrEmpty ();
			}
		}

		public Collection<CustomAttributeArgument> ConstructorArguments {
			get {
				Resolve ();

				if (arguments == null)
					Interlocked.CompareExchange (ref arguments, new Collection<CustomAttributeArgument> (), null);

				return arguments;
			}
		}

		public bool HasFields {
			get {
				Resolve ();

				return !fields.IsNullOrEmpty ();
			}
		}

		public Collection<CustomAttributeNamedArgument> Fields {
			get {
				Resolve ();

				if (fields == null)
					Interlocked.CompareExchange (ref fields, new Collection<CustomAttributeNamedArgument> (), null);

				return fields;
			}
		}

		public bool HasProperties {
			get {
				Resolve ();

				return !properties.IsNullOrEmpty ();
			}
		}

		public Collection<CustomAttributeNamedArgument> Properties {
			get {
				Resolve ();

				if (properties == null)
					Interlocked.CompareExchange (ref properties, new Collection<CustomAttributeNamedArgument> (), null);

				return properties;
			}
		}

		internal bool HasImage {
			get { return constructor != null && constructor.HasImage; }
		}

		internal ModuleDefinition Module {
			get { return constructor.Module; }
		}

		internal CustomAttribute (uint signature, MethodReference constructor)
		{
			this.signature = signature;
			this.constructor = constructor;
			this.resolved = false;
		}

		public CustomAttribute (MethodReference constructor)
		{
			this.constructor = constructor;
			this.resolved = true;
		}

		public CustomAttribute (MethodReference constructor, byte [] blob)
		{
			this.constructor = constructor;
			this.resolved = false;
			this.blob = blob;
		}

		public byte [] GetBlob ()
		{
			if (blob != null)
				return blob;

			if (!HasImage)
				throw new NotSupportedException ();

			return Module.Read (ref blob, this, (attribute, reader) => reader.ReadCustomAttributeBlob (attribute.signature));
		}

		void Resolve ()
		{
			if (resolved || !HasImage)
				return;

			lock (Module.SyncRoot) {
				if (resolved)
					return;

				Module.Read (this, (attribute, reader) => {
					try {
						reader.ReadCustomAttributeSignature (attribute);
						resolved = true;
					} catch (ResolutionException) {
						if (arguments != null)
							arguments.Clear ();
						if (fields != null)
							fields.Clear ();
						if (properties != null)
							properties.Clear ();

						resolved = false;
					}
				});
			}
		}
	}
}
